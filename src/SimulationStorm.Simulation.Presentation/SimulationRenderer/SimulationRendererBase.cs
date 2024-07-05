using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DotNext.Threading;
using SimulationStorm.Graphics;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public abstract class SimulationRendererBase : RendererBase, ISimulationRenderer
{
    #region Properties
    public bool IsRenderingEnabled
    {
        get => _intervalActionExecutor.IsEnabled;
        set => SetProperty(_intervalActionExecutor.IsEnabled, value, _intervalActionExecutor, (o, v) => o.IsEnabled = v);
    }

    public int RenderingInterval
    {
        get => _intervalActionExecutor.Interval;
        set => SetProperty(_intervalActionExecutor.Interval, value, _intervalActionExecutor, (o, v) => o.Interval = v);
    }
    #endregion

    public new event EventHandler<SimulationRenderingCompletedEventArgs>? RenderingCompleted;

    #region Fields
    private readonly IIntervalActionExecutor _intervalActionExecutor;

    private readonly AsyncAutoResetEvent _commandCompletedEventSynchronizer = new(false);
    
    private readonly ConcurrentQueue<SimulationCommand> _commandQueue = new();
    #endregion
    
    protected SimulationRendererBase
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IIntervalActionExecutor intervalActionExecutor,
        ISimulationRendererOptions options
    )
        : base(graphicsFactory, benchmarker)
    {
        _intervalActionExecutor = intervalActionExecutor;

        IsRenderingEnabled = options.IsRenderingEnabled;
        RenderingInterval = options.RenderingInterval;
    }

    public async Task HandleSimulationCommandCompletedAsync(SimulationCommandCompletedEventArgs e)
    {
        if (!IsRenderingNeeded(e.Command))
            return;

        RememberCompletedCommand(e.Command);
        
        RequestRerender();

        await _commandCompletedEventSynchronizer
            .WaitAsync()
            .ConfigureAwait(false);
    }

    #region Protected methods
    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore()
            .ConfigureAwait(false);
        
        await _commandCompletedEventSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
    }

    protected override async Task RenderAndNotifyStartingAndCompletedAsync()
    {
        await base
            .RenderAndNotifyStartingAndCompletedAsync()
            .ConfigureAwait(false);

        _commandCompletedEventSynchronizer.Set();
    }

    protected override void NotifyRenderingCompleted(TimeSpan elapsedTime)
    {
        var command = GetFirstRememberedCompletedCommand();
        RenderingCompleted?.Invoke(this, new SimulationRenderingCompletedEventArgs(command, elapsedTime));
    }
    #endregion

    #region Private methods
    private void RememberCompletedCommand(SimulationCommand command) => _commandQueue.Enqueue(command);

    private SimulationCommand? GetFirstRememberedCompletedCommand()
    {
        _commandQueue.TryDequeue(out var command);
        return command;
    }
    
    private bool IsRenderingNeeded(SimulationCommand command) =>
        !IsDisposingOrDisposed
        && command.ChangesWorld
        && _intervalActionExecutor.GetIsExecutionNeededAndMoveNext();
    #endregion
}