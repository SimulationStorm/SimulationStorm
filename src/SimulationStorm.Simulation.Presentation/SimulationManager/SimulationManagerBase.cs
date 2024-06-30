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
    #region Events
    public event EventHandler<SimulationCommandEventArgs>? CommandScheduled;
    
    public event EventHandler<SimulationCommandEventArgs>? CommandStarting;
    
    public event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion

    #region Fields
    private readonly AsyncReaderWriterLock _readerWriterLock = new();
    
    private readonly IBenchmarkingService _benchmarkingService;

    private Channel<ScheduledSimulationCommand> _scheduledCommandChannel;

    private CancellationTokenSource _commandProcessingCycleCts = new();

    private Task _commandProcessingCycleTask;

    private readonly AsyncCountdownEvent _commandCompletedEventSynchronizer;
    #endregion

    protected SimulationManagerBase
    (
        IBenchmarkingService benchmarkingService,
        ISimulationManagerOptions options)
    {
        _benchmarkingService = benchmarkingService;
        
        _scheduledCommandChannel = Channel.CreateUnbounded<ScheduledSimulationCommand>(new UnboundedChannelOptions
        {
            SingleReader = true
        });
        
        _commandProcessingCycleTask = ProcessCommandsInCycleAsync(_commandProcessingCycleCts.Token);
        
        _commandCompletedEventSynchronizer = new AsyncCountdownEvent(options.CommandExecutedEventHandlerCount);
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
                
                NotifyCommandStarting(queuedCommand.Command);
                var elapsedTime = Measure(() => ExecuteCommand(queuedCommand.Command));
                
                _readerWriterLock.Release();

                queuedCommand.NotifyTaskCompleted();

                NotifyCommandCompleted(queuedCommand.Command, elapsedTime);
                
                await _commandCompletedEventSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);

                _commandCompletedEventSynchronizer.Reset();
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
    
    private void NotifyCommandStarting(SimulationCommand command) =>
        CommandStarting?.Invoke(this, new SimulationCommandEventArgs(command));

    private void NotifyCommandCompleted(SimulationCommand command, TimeSpan elapsedTime) =>
        CommandCompleted?.Invoke(this, new SimulationCommandCompletedEventArgs(
            command, elapsedTime, _commandCompletedEventSynchronizer));
    #endregion
}