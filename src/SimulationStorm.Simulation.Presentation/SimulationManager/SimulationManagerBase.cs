using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DotNext.Threading;
using SimulationStorm.Utilities.Benchmarking;
using SimulationStorm.Utilities.Disposing;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public abstract partial class SimulationManagerBase : AsyncDisposableObject, ISimulationManager
{
    public bool IsCommandProgressReportingEnabled { get; set; }

    #region Events
    public event EventHandler<SimulationCommandEventArgs>? CommandScheduled;
    
    public event EventHandler<SimulationCommandEventArgs>? CommandStarting;
    
    public event EventHandler<SimulationCommandProgressChangedEventArgs>? CommandProgressChanged;
    
    public event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion

    #region Fields
    private readonly IBenchmarkingService _benchmarkingService;
    
    private ISimulation _simulation = null!;
    
    private readonly Stopwatch _stopwatch = new();

    private readonly AsyncReaderWriterLock _scheduledCommandChannelRwLock = new();

    private Channel<ScheduledCommand> _scheduledCommandChannel = null!;

    private readonly CancellationTokenSource _commandProcessingCycleCts = new();
    
    private readonly AsyncReaderWriterLock _simulationRwLock = new();

    private readonly AsyncCountdownEvent _commandCompletedEventSynchronizer;

    // This field is needed to store command while doing ExecuteCommand to access command from simulation progress event
    private SimulationCommand? _executingCommand;
    #endregion

    protected SimulationManagerBase(IBenchmarkingService benchmarkingService, ISimulationManagerOptions options)
    {
        _benchmarkingService = benchmarkingService;
        _commandCompletedEventSynchronizer = new AsyncCountdownEvent(options.CommandExecutedEventHandlerCount);
        
        InitializeCommandChannelAndStartProcessing();
    }

    #region Public methods
    public async Task ScheduleCommandAsync(SimulationCommand command)
    {
        ThrowIfDisposingOrDisposed();

        await _scheduledCommandChannelRwLock
            .EnterReadLockAsync()
            .ConfigureAwait(false);

        var scheduledCommand = new ScheduledCommand(command);

        await _scheduledCommandChannel.Writer
            .WriteAsync(scheduledCommand)
            .ConfigureAwait(false);
        
        NotifyCommandScheduled(command);
        
        _scheduledCommandChannelRwLock.Release();
        
        await scheduledCommand.Task
            .ConfigureAwait(false);
    }

    public async Task ClearScheduledCommandsAsync()
    {
        ThrowIfDisposingOrDisposed();
        
        await _scheduledCommandChannelRwLock
            .EnterWriteLockAsync()
            .ConfigureAwait(false);
        
        _scheduledCommandChannel.Writer.Complete();
        
        await _commandProcessingCycleCts
            .CancelAsync()
            .ConfigureAwait(false);

        InitializeCommandChannelAndStartProcessing();

        _scheduledCommandChannelRwLock.Release();
    }
    #endregion

    #region Protected methods
    protected abstract void ExecuteCommand(SimulationCommand command);
    
    protected void ResetSimulationInstance(ISimulation simulation)
    {
        if (_simulation is not null)
            _simulation.ProgressChanged -= OnSimulationProgressChanged;
        
        _simulation = simulation;
        _simulation.ProgressChanged += OnSimulationProgressChanged;
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        _scheduledCommandChannel.Writer.Complete();
        
        await _commandProcessingCycleCts
            .CancelAsync()
            .ConfigureAwait(false);

        // In this new implementation, we try to avoid waiting task itself...
        
        await _commandCompletedEventSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);

        await _simulationRwLock
            .DisposeAsync()
            .ConfigureAwait(false);
    }

    #region Benchmarking helpers
    protected Task<BenchmarkResult> MeasureWithSimulationReadLockAsync(Action action) =>
        WithSimulationReadLockAsync(() => _benchmarkingService.Measure(action));
    
    protected Task<BenchmarkResult<T>> MeasureWithSimulationReadLockAsync<T>(Func<T> function) =>
        WithSimulationReadLockAsync(() => _benchmarkingService.Measure(function));
    
    protected Task<BenchmarkResult> MeasureWithSimulationWriteLockAsync(Action action) =>
        WithSimulationWriteLockAsync(() => _benchmarkingService.Measure(action));
    
    protected Task<BenchmarkResult<T>> MeasureWithSimulationWriteLockAsync<T>(Func<T> function) =>
        WithSimulationWriteLockAsync(() => _benchmarkingService.Measure(function));
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
    private void InitializeCommandChannelAndStartProcessing()
    {
        InitializeScheduledCommandChannel();
        StartScheduledCommandsProcessing();
    }
    
    private void InitializeScheduledCommandChannel() => _scheduledCommandChannel =
        Channel.CreateUnbounded<ScheduledCommand>(new UnboundedChannelOptions
        {
            SingleReader = true
        });
    
    private void StartScheduledCommandsProcessing() =>
        ProcessScheduledCommandsAsync(_commandProcessingCycleCts.Token)
            .ConfigureAwait(false);
    
    private async Task ProcessScheduledCommandsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var scheduledCommand in _scheduledCommandChannel.Reader
                               .ReadAllAsync(cancellationToken)
                               .ConfigureAwait(false))
            {
                await ProcessScheduledCommand(scheduledCommand, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException _)
        {
            // No operation
        }
    }
    
    #region Scheduled command processing
    private async Task ProcessScheduledCommand(ScheduledCommand scheduledCommand, CancellationToken cancellationToken)
    {
        var command = scheduledCommand.Command;

        var elapsedTime = await NotifyCommandStartedAndExecuteCommandAsync(command, cancellationToken)
            .ConfigureAwait(false);

        await NotifyCommandCompletedAndWaitForEventHandling(command, elapsedTime, cancellationToken)
            .ConfigureAwait(false);
        
        scheduledCommand.NotifyTaskCompleted();
    }

    private async Task<TimeSpan> NotifyCommandStartedAndExecuteCommandAsync(
        SimulationCommand command, CancellationToken cancellationToken)
    {
        await _simulationRwLock
            .EnterWriteLockAsync(cancellationToken)
            .ConfigureAwait(false);
        
        NotifyCommandStarting(command);

        // The simplest way to synchronize this property is to set it before command execution
        _simulation.IsProgressReportingEnabled = IsCommandProgressReportingEnabled;
        
        var elapsedTime = ExecuteCommandCore(command);
        _simulationRwLock.Release();
        return elapsedTime;
    }
    
    private TimeSpan ExecuteCommandCore(SimulationCommand command)
    {
        _executingCommand = command;
        _stopwatch.Restart();
        ExecuteCommand(command);
        _stopwatch.Stop();
        _executingCommand = null;
        
        return _stopwatch.Elapsed;
    }

    private void OnSimulationProgressChanged(object? _, CancellableProgressChangedEventArgs e)
    {
        _stopwatch.Stop();

        // If disposing / clearing commands, cancel current simulation domain operation and return...
        if (_commandProcessingCycleCts.IsCancellationRequested)
        {
            e.Cancel = true;
            return;
        }
            
        NotifyCommandProgressChanged(_executingCommand!, e.ProgressPercentage);
            
        if (e.ProgressPercentage is not 100) // Todo: this if may be omitted
            _stopwatch.Start();
    }

    private async ValueTask NotifyCommandCompletedAndWaitForEventHandling(
        SimulationCommand command, TimeSpan elapsedTime, CancellationToken cancellationToken)
    {
        NotifyCommandCompleted(command, elapsedTime);

        await _commandCompletedEventSynchronizer
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        _commandCompletedEventSynchronizer.Reset();
    }
    #endregion

    #region Notification methods
    private void NotifyCommandScheduled(SimulationCommand command) =>
        CommandScheduled?.Invoke(this, new SimulationCommandEventArgs(command));
    
    private void NotifyCommandStarting(SimulationCommand command) =>
        CommandStarting?.Invoke(this, new SimulationCommandEventArgs(command));

    private void NotifyCommandProgressChanged(SimulationCommand command, int progressPercentage) =>
        CommandProgressChanged?.Invoke(this, new SimulationCommandProgressChangedEventArgs(command, progressPercentage));

    private void NotifyCommandCompleted(SimulationCommand command, TimeSpan elapsedTime) =>
        CommandCompleted?.Invoke(this, new SimulationCommandCompletedEventArgs(
            command, elapsedTime, _commandCompletedEventSynchronizer));
    #endregion
    #endregion
}