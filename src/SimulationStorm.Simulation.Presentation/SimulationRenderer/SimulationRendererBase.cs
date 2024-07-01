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
    
    private SimulationCommandCompletedEventArgs? _simulationCommandExecutedEvent;

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
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => simulationManager.CommandCompleted += h,
                    h => simulationManager.CommandCompleted -= h
                )
                .Select(e => e.EventArgs)
                .Subscribe(e =>
                {
                    _simulationCommandExecutedEvent = e;
                    
                    if (!e.Command.ChangesWorld)
                    {
                        SignalSimulationCommandExecutedEventHandled();
                        return;
                    }

                    if (_intervalActionExecutor.GetIsExecutionNeededAndMoveNext())
                    {
                        RememberCommand(e.Command);
                        RequestRerender();
                    }
                    else
                        SignalSimulationCommandExecutedEventHandled();
                })
                .DisposeWith(disposables);
        });
    }
    
    protected override async Task RenderAndNotifyStartingAndCompletedAsync()
    {
        await base
            .RenderAndNotifyStartingAndCompletedAsync()
            .ConfigureAwait(false);
        
        SignalSimulationCommandExecutedEventHandled();
    }

    protected void RememberCommand(SimulationCommand command) => _commandQueue.Enqueue(command);

    protected override void NotifyRenderingCompleted(TimeSpan elapsedTime)
    {
        var command = GetFirstRememberedCommand();
        RenderingCompleted?.Invoke(this, new SimulationRenderingCompletedEventArgs(command, elapsedTime));
    }
    
    private SimulationCommand? GetFirstRememberedCommand()
    {
        _commandQueue.TryDequeue(out var command);
        return command;
    }

    private void SignalSimulationCommandExecutedEventHandled()
    {
        _simulationCommandExecutedEvent?.Synchronizer.Signal();
        _simulationCommandExecutedEvent = null;
    }
}