using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    
    private SimulationCommandCompletedEventArgs? _simulationCommandCompletedEventArgs;

    private readonly ConcurrentQueue<SimulationCommand> _commandQueue = new();
    #endregion
    
    protected SimulationRendererBase
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IIntervalActionExecutor intervalActionExecutor,
        ISimulationManager simulationManager,
        ISimulationRendererOptions options
    )
        : base(graphicsFactory, benchmarker)
    {
        _intervalActionExecutor = intervalActionExecutor;

        IsRenderingEnabled = options.IsRenderingEnabled;
        RenderingInterval = options.RenderingInterval;
        
        simulationManager
            .CommandCompletedObservable()
            .Subscribe(e =>
            {
                _simulationCommandCompletedEventArgs = e;
                
                if (!e.Command.ChangesWorld)
                {
                    SignalSimulationCommandCompletedEventHandled();
                    return;
                }

                if (_intervalActionExecutor.GetIsExecutionNeededAndMoveNext())
                {
                    RememberCompletedCommand(e.Command);
                    RequestRerender();
                }
                else
                    SignalSimulationCommandCompletedEventHandled();
            })
            .DisposeWith(Disposables);
    }

    #region Protected methods
    protected override async Task RenderAndNotifyStartingAndCompletedAsync()
    {
        await base
            .RenderAndNotifyStartingAndCompletedAsync()
            .ConfigureAwait(false);
        
        SignalSimulationCommandCompletedEventHandled();
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

    private void SignalSimulationCommandCompletedEventHandled()
    {
        _simulationCommandCompletedEventArgs?.Synchronizer.Signal();
        _simulationCommandCompletedEventArgs = null;
    }
    #endregion
}