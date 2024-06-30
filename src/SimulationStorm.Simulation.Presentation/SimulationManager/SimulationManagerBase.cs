using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DotNext.Threading;
using SimulationStorm.Utilities.Benchmarking;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public abstract class SimulationManagerBase : AsyncDisposableObject, ISimulationManager
{
    public ReadOnlyObservableCollection<SimulationCommand> ScheduledCommandQueue { get; }
    
    #region Events
    public event EventHandler<SimulationCommandEventArgs>? CommandScheduled;
    
    public event EventHandler<SimulationCommandEventArgs>? CommandStarting;
    
    public event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion

    #region Fields
    private readonly AsyncReaderWriterLock _readerWriterLock = new();
    
    private readonly IBenchmarkingService _benchmarkingService;

    private readonly ISimulationManagerOptions _options;
    
    private Channel<ScheduledSimulationCommand> _scheduledCommandChannel;

    private CancellationTokenSource _commandProcessingCycleCts = new();

    private Task _commandProcessingCycleTask;

    private readonly ObservableCollection<SimulationCommand> _scheduledCommands = [];
    #endregion

    protected SimulationManagerBase
    (
        IBenchmarkingService benchmarkingService,
        ISimulationManagerOptions options)
    {
        _benchmarkingService = benchmarkingService;
        _options = options;
        
        _scheduledCommandChannel = Channel.CreateUnbounded<ScheduledSimulationCommand>(new UnboundedChannelOptions
        {
            SingleReader = true
        });
        
        ScheduledCommandQueue = new ReadOnlyObservableCollection<SimulationCommand>(_scheduledCommands);
        
        _commandProcessingCycleTask = ProcessCommandsInCycleAsync(_commandProcessingCycleCts.Token);
    }

    public override async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;
        
        IsDisposed = true;
        
        _scheduledCommandChannel.Writer.Complete();
        
        await _commandProcessingCycleCts
            .CancelAsync()
            .ConfigureAwait(false);
        
        await _commandProcessingCycleTask
            .ConfigureAwait(false);
        
        await _readerWriterLock
            .DisposeAsync()
            .ConfigureAwait(false);
        
        _commandProcessingCycleCts.Dispose();

        await base
            .DisposeAsync()
            .ConfigureAwait(false);
        
        GC.SuppressFinalize(this);
    }

    #region Protected methods
    protected abstract void ExecuteCommand(SimulationCommand command);

    protected Task<TimeSpan> MeasureWithReadLockAsync(Action action) =>
        WithReadLockAsync(() => Measure(action));

    protected Task<BenchmarkResult<T>> MeasureWithReadLockAsync<T>(Func<T> function) =>
        WithReadLockAsync(() => _benchmarkingService.Measure(function));

    protected async Task WithReadLockAsync(Action action)
    {
        await _readerWriterLock
            .EnterReadLockAsync()
            .ConfigureAwait(false);
        
        action();
        
        _readerWriterLock.Release();
    }

    protected async Task<T> WithReadLockAsync<T>(Func<T> function)
    {
        await _readerWriterLock
            .EnterReadLockAsync()
            .ConfigureAwait(false);
        
        var functionResult = function();
        _readerWriterLock.Release();
        return functionResult;
    }
    
    protected TimeSpan Measure(Action action) => _benchmarkingService.Measure(action).ElapsedTime;

    protected BenchmarkResult<T> Measure<T>(Func<T> function) => _benchmarkingService.Measure(function);

    protected async Task QueueCommandExecutionAsync(SimulationCommand command)
    {
        // Todo: At the moment, do so to avoid exception with object disposed exception
        if (IsDisposed)
            return;
        
        // await _commandChannelRwLock.EnterReadLockAsync();
        
        var scheduledCommand = new ScheduledSimulationCommand(command);
        NotifyCommandScheduled(command);
        
        _scheduledCommands.Add(command);
        
        await _scheduledCommandChannel.Writer
            .WriteAsync(scheduledCommand)
            .ConfigureAwait(false);
        
        // _commandChannelRwLock.Release();
        
        await scheduledCommand.Task
            .ConfigureAwait(false);
    }
    #endregion
    
    private async Task ProcessCommandsInCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var queuedCommand in _scheduledCommandChannel.Reader
                               .ReadAllAsync(cancellationToken)
                               .ConfigureAwait(false))
            {
                await _readerWriterLock
                    .EnterWriteLockAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                NotifyCommandExecuting(queuedCommand.Command);
                var elapsedTime = Measure(() => ExecuteCommand(queuedCommand.Command));
                
                _readerWriterLock.Release();

                _scheduledCommands.RemoveAt(0);
                queuedCommand.NotifyTaskCompleted();

                await using var eventSynchronizer = new AsyncCountdownEvent(_options.CommandExecutedEventHandlerCount);
                NotifyCommandExecuted(queuedCommand.Command, elapsedTime, eventSynchronizer);
                
                await eventSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException _)
        {
            // Do nothing
        }
    }

    #region Notifying
    private void NotifyCommandScheduled(SimulationCommand command) =>
        CommandScheduled?.Invoke(this, new SimulationCommandEventArgs(command));
    
    private void NotifyCommandExecuting(SimulationCommand command) =>
        CommandStarting?.Invoke(this, new SimulationCommandEventArgs(command));

    private void NotifyCommandExecuted(SimulationCommand command, TimeSpan elapsedTime, IAsyncEvent synchronizer) =>
        CommandCompleted?.Invoke(this, new SimulationCommandCompletedEventArgs(command, elapsedTime, synchronizer));
    #endregion
}