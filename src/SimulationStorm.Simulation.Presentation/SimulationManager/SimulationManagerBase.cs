using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DotNext.Collections.Generic;
using DotNext.Threading;
using SimulationStorm.Utilities.Benchmarking;
using SimulationStorm.Utilities.Disposing;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public abstract partial class SimulationManagerBase : AsyncDisposableObject, ISimulationManager
{
    public bool IsCommandProgressReportingEnabled { get; set; }

    #region Events
    public event EventHandler<SimulationCommandEventArgs>? CommandScheduling;
    
    public event EventHandler<SimulationCommandEventArgs>? CommandStarting;
    
    public event EventHandler<SimulationCommandProgressChangedEventArgs>? CommandProgressChanged;
    
    public event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion

    #region Fields
    private readonly IBenchmarker _benchmarker;

    private readonly ICollection<ISimulationCommandCompletedHandler> _commandCompletedHandlers =
        new Collection<ISimulationCommandCompletedHandler>();
    
    private ISimulation? _simulation;
    
    private readonly AsyncReaderWriterLock _simulationRwLock = new();

    private readonly AsyncExclusiveLock _commandChannelLock = new();

    #region Command processing
    private CancellationTokenSource _commandProcessingLoopCts = new();

    private Task _commandProcessingLoopTask = null!;

    private Channel<ScheduledCommand> _commandChannel = null!;

    private readonly Collection<ScheduledCommand> _scheduledCommands = [];
    #endregion

    #region Command execution
    private readonly Stopwatch _commandStopwatch = new();
    
    private SimulationCommand? _executingCommand;
    #endregion
    #endregion

    protected SimulationManagerBase(IBenchmarker benchmarker)
    {
        _benchmarker = benchmarker;
        
        InitializeCommandChannelAndStartProcessingLoop();
    }

    #region Public methods
    public void AddCommandCompletedHandler(ISimulationCommandCompletedHandler commandCompletedHandler) =>
        _commandCompletedHandlers.Add(commandCompletedHandler);

    public async Task ScheduleCommandAsync(SimulationCommand command)
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);

        var scheduledCommand = await ScheduleCommandAndNotifyAsync(command)
            .ConfigureAwait(false);
        
        await scheduledCommand.Task
            .ConfigureAwait(false);
    }

    public async Task ClearScheduledCommandsAsync()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        await _commandChannelLock
            .AcquireAsync()
            .ConfigureAwait(false);

        await StopCommandProcessingLoopAsync()
            .ConfigureAwait(false);

        InitializeCommandChannelAndStartProcessingLoop();

        _commandChannelLock.Release();
    }
    #endregion

    #region Protected methods
    protected abstract void ExecuteCommand(SimulationCommand command);
    
    protected void ResetSimulationInstance(ISimulation simulation)
    {
        if (_simulation is not null)
            _simulation.OperationProgressChanged -= OnSimulationOperationProgressChanged;
        
        _simulation = simulation;
        _simulation.OperationProgressChanged += OnSimulationOperationProgressChanged;
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _commandChannelLock
            .DisposeAsync()
            .ConfigureAwait(false);
        
        await StopCommandProcessingLoopAsync()
            .ConfigureAwait(false);
        
        await _simulationRwLock
            .DisposeAsync()
            .ConfigureAwait(false);
    }

    #region Benchmarking helpers
    protected Task<BenchmarkResult> MeasureWithSimulationReadLockAsync(Action action) =>
        WithSimulationReadLockAsync(() => _benchmarker.Measure(action));
    
    protected Task<BenchmarkResult<T>> MeasureWithSimulationReadLockAsync<T>(Func<T> function) =>
        WithSimulationReadLockAsync(() => _benchmarker.Measure(function));
    
    protected Task<BenchmarkResult> MeasureWithSimulationWriteLockAsync(Action action) =>
        WithSimulationWriteLockAsync(() => _benchmarker.Measure(action));
    
    protected Task<BenchmarkResult<T>> MeasureWithSimulationWriteLockAsync<T>(Func<T> function) =>
        WithSimulationWriteLockAsync(() => _benchmarker.Measure(function));
    #endregion
    
    #region Simulation read/write lock helpers
    protected async Task WithSimulationReadLockAsync(Action action)
    {
        await _simulationRwLock
            .EnterReadLockAsync()
            .ConfigureAwait(false);
        
        action();
        
        _simulationRwLock.Release();
    }
    
    protected async Task WithSimulationWriteLockAsync(Action action)
    {
        await _simulationRwLock
            .EnterWriteLockAsync()
            .ConfigureAwait(false);
        
        action();
        
        _simulationRwLock.Release();
    }

    protected async Task<T> WithSimulationReadLockAsync<T>(Func<T> function)
    {
        await _simulationRwLock
            .EnterReadLockAsync()
            .ConfigureAwait(false);
        
        var functionResult = function();
        
        _simulationRwLock.Release();
        
        return functionResult;
    }
    
    protected async Task<T> WithSimulationWriteLockAsync<T>(Func<T> function)
    {
        await _simulationRwLock
            .EnterWriteLockAsync()
            .ConfigureAwait(false);
        
        var functionResult = function();
        
        _simulationRwLock.Release();
        
        return functionResult;
    }
    #endregion
    #endregion
    
    #region Private methods
    private void InitializeCommandChannelAndStartProcessingLoop()
    {
        InitializeCommandChannel();
        StartCommandProcessingLoop();
    }
    
    private void InitializeCommandChannel() => _commandChannel =
        Channel.CreateUnbounded<ScheduledCommand>(new UnboundedChannelOptions
        {
            SingleReader = true
        });
    
    private void StartCommandProcessingLoop()
    {
        _commandProcessingLoopCts = new CancellationTokenSource();
        _commandProcessingLoopTask = ProcessScheduledCommandsAsync(_commandProcessingLoopCts.Token);
    }

    private async Task StopCommandProcessingLoopAsync()
    {
        _commandChannel.Writer.Complete();
        
        await _commandProcessingLoopCts
            .CancelAsync()
            .ConfigureAwait(false);

        await _commandProcessingLoopTask
            .ConfigureAwait(false);
        
        _scheduledCommands.ForEach(scheduledCommand => scheduledCommand.NotifyCanceled());
        _scheduledCommands.Clear();
    }

    private async Task<ScheduledCommand> ScheduleCommandAndNotifyAsync(SimulationCommand command)
    {
        NotifyCommandScheduling(command);
        
        await _commandChannelLock
            .AcquireAsync()
            .ConfigureAwait(false);

        var scheduledCommand = new ScheduledCommand(command);
        
        _scheduledCommands.Add(scheduledCommand);
        
        await _commandChannel.Writer
            .WriteAsync(scheduledCommand)
            .ConfigureAwait(false);
        
        _commandChannelLock.Release();

        return scheduledCommand;
    }
    
    private async Task ProcessScheduledCommandsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var scheduledCommand in _commandChannel.Reader
                               .ReadAllAsync(cancellationToken)
                               .ConfigureAwait(false))
            {
                await ProcessScheduledCommand(scheduledCommand, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) { }
    }
    
    #region Scheduled command processing
    private async Task ProcessScheduledCommand(ScheduledCommand scheduledCommand, CancellationToken cancellationToken)
    {
        var command = scheduledCommand.Command;

        var elapsedTime = await NotifyCommandStartingAndExecuteCommandAsync(command, cancellationToken)
            .ConfigureAwait(false);

        await InvokeCommandCompletedHandlersAndNotify(command, elapsedTime)
            .ConfigureAwait(false);
        
        scheduledCommand.NotifyCompleted();

        _scheduledCommands.Remove(scheduledCommand);
    }

    private async Task<TimeSpan> NotifyCommandStartingAndExecuteCommandAsync(
        SimulationCommand command, CancellationToken cancellationToken)
    {
        NotifyCommandStarting(command);
        
        await _simulationRwLock
            .EnterWriteLockAsync(cancellationToken)
            .ConfigureAwait(false);

        _simulation!.IsOperationProgressReportingEnabled = IsCommandProgressReportingEnabled;
        
        var elapsedTime = ExecuteCommandCore(command);
        
        _simulationRwLock.Release();
        
        return elapsedTime;
    }
    
    private TimeSpan ExecuteCommandCore(SimulationCommand command)
    {
        _executingCommand = command;
        _commandStopwatch.Restart();
        ExecuteCommand(command);
        _commandStopwatch.Stop();
        _executingCommand = null;
        
        return _commandStopwatch.Elapsed;
    }

    private void OnSimulationOperationProgressChanged(object? _, CancellableProgressChangedEventArgs e)
    {
        _commandStopwatch.Stop();

        if (_commandProcessingLoopCts.IsCancellationRequested)
        {
            e.Cancel = true;
            return;
        }
            
        NotifyCommandProgressChanged(_executingCommand!, e.ProgressPercentage);
            
        if (e.ProgressPercentage is not 100) // Todo: this if may be omitted
            _commandStopwatch.Start();
    }

    private async ValueTask InvokeCommandCompletedHandlersAndNotify(SimulationCommand command, TimeSpan elapsedTime)
    {
        var eventArgs = new SimulationCommandCompletedEventArgs(command, elapsedTime);

        foreach (var commandCompletedEventHandler in _commandCompletedHandlers)
            await commandCompletedEventHandler
                .HandleSimulationCommandCompletedAsync(eventArgs)
                .ConfigureAwait(false);
        
        NotifyCommandCompleted(eventArgs);
    }
    #endregion

    #region Notification methods
    private void NotifyCommandScheduling(SimulationCommand command) =>
        CommandScheduling?.Invoke(this, new SimulationCommandEventArgs(command));
    
    private void NotifyCommandStarting(SimulationCommand command) =>
        CommandStarting?.Invoke(this, new SimulationCommandEventArgs(command));

    private void NotifyCommandProgressChanged(SimulationCommand command, int progressPercentage) =>
        CommandProgressChanged?.Invoke(this, new SimulationCommandProgressChangedEventArgs(command, progressPercentage));

    private void NotifyCommandCompleted(SimulationCommandCompletedEventArgs eventArgs) =>
        CommandCompleted?.Invoke(this, eventArgs);
    #endregion
    #endregion
}