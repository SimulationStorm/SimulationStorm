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
    public ReadOnlyObservableCollection<SimulationCommand> CommandQueue { get; }
    
    #region Events
    public event EventHandler<SimulationCommandEventArgs>? CommandStarting;
    
    public event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion

    #region Fields
    private readonly AsyncReaderWriterLock _readerWriterLock = new();
    
    private readonly IBenchmarkingService _benchmarkingService;

    private readonly ISimulationManagerOptions _options;
    
    private Channel<QueuedSimulationCommand> _commandChannel;

    private CancellationTokenSource _commandProcessingCycleCts = new();

    private Task _commandProcessingCycleTask;

    private readonly ObservableCollection<SimulationCommand> _commandQueue = [];
    #endregion

    protected SimulationManagerBase
    (
        IBenchmarkingService benchmarkingService,
        ISimulationManagerOptions options)
    {
        _benchmarkingService = benchmarkingService;
        _options = options;
        
        _commandChannel = Channel.CreateUnbounded<QueuedSimulationCommand>(new UnboundedChannelOptions
        {
            SingleReader = true
        });
        
        CommandQueue = new ReadOnlyObservableCollection<SimulationCommand>(_commandQueue);
        
        _commandProcessingCycleTask = ProcessCommandsInCycleAsync(_commandProcessingCycleCts.Token);
    }

    // private readonly AsyncReaderWriterLock _commandChannelRwLock = new();
    
    // public async Task ClearCommandQueueAsync()
    // {
    //     await _commandChannelRwLock.EnterWriteLockAsync();
    //     
    //     _commandChannel.Writer.Complete();
    //     await _commandProcessingCycleCts.CancelAsync();
    //     
    //     await _commandProcessingCycleTask;
    //     _commandProcessingCycleCts.Dispose();
    //     
    //     _commandChannel = Channel.CreateUnbounded<QueuedSimulationCommand>(new UnboundedChannelOptions
    //     {
    //         SingleReader = true
    //     });
    //
    //     _commandProcessingCycleCts = new CancellationTokenSource();
    //     _commandProcessingCycleTask = ProcessCommandsInCycleAsync(_commandProcessingCycleCts.Token)
    //         .ThrowWhenFaulted();
    //     
    //     _commandChannelRwLock.Release();
    // }

    public override async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;
        
        IsDisposed = true;
        
        _commandChannel.Writer.Complete();
        
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
        
        var queuedCommand = new QueuedSimulationCommand(command);
        
        _commandQueue.Add(command);
        
        await _commandChannel.Writer
            .WriteAsync(queuedCommand)
            .ConfigureAwait(false);
        
        // _commandChannelRwLock.Release();
        
        await queuedCommand.Task
            .ConfigureAwait(false);
    }
    #endregion
    
    private async Task ProcessCommandsInCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var queuedCommand in _commandChannel.Reader
                               .ReadAllAsync(cancellationToken)
                               .ConfigureAwait(false))
            {
                await _readerWriterLock
                    .EnterWriteLockAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                NotifyCommandExecuting(queuedCommand.Command);
                var elapsedTime = Measure(() => ExecuteCommand(queuedCommand.Command));
                
                _readerWriterLock.Release();

                _commandQueue.RemoveAt(0);
                queuedCommand.NotifyExecuted();

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
    private void NotifyCommandExecuting(SimulationCommand command) =>
        CommandStarting?.Invoke(this, new SimulationCommandEventArgs(command));

    private void NotifyCommandExecuted(SimulationCommand command, TimeSpan elapsedTime, IAsyncEvent synchronizer) =>
        CommandCompleted?.Invoke(this, new SimulationCommandCompletedEventArgs(command, elapsedTime, synchronizer));
    #endregion
}