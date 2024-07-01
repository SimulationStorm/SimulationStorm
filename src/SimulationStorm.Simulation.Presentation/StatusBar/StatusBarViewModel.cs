using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.StatusBar;

public class StatusBarViewModel : DisposableObservableObject
{
    #region Properties
    #region Simulation command execution
    private bool _isCommandProgressVisible;
    public bool IsCommandProgressVisible
    {
        get => _isCommandProgressVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isCommandProgressVisible, value));
    }
    
    private bool _isCommandTimeVisible;
    public bool IsCommandTimeVisible
    {
        get => _isCommandTimeVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isCommandTimeVisible, value));
    }

    private SimulationCommand? _executingCommand;
    public SimulationCommand? ExecutingCommand
    {
        get => _executingCommand;
        set => _uiThreadScheduler.Schedule(() =>
        {
            if (SetProperty(ref _executingCommand, value))
                OnPropertyChanged(nameof(IsCommandExecuting));
        });
    }
    
    public bool IsCommandExecuting => ExecutingCommand is not null;

    private TimeSpan _commandTime;
    public TimeSpan CommandTime
    {
        get => _commandTime;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _commandTime, value));
    }
    #endregion

    #region Simulation rendering
    private bool _isSimulationRenderingProgressVisible;
    public bool IsSimulationRenderingProgressVisible
    {
        get => _isSimulationRenderingProgressVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isSimulationRenderingProgressVisible, value));
    }
    
    private bool _isSimulationRenderingInProgress;
    public bool IsSimulationRenderingInProgress
    {
        get => _isSimulationRenderingInProgress;
        private set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isSimulationRenderingInProgress, value));
    }
    
    private bool _isSimulationRenderingTimeVisible;
    public bool IsSimulationRenderingTimeVisible
    {
        get => _isSimulationRenderingTimeVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isSimulationRenderingTimeVisible, value));
    }

    private TimeSpan _simulationRenderingTime;
    public TimeSpan SimulationRenderingTime
    {
        get => _simulationRenderingTime;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _simulationRenderingTime, value));
    }
    #endregion

    #region World rendering
    private bool _isWorldRenderingTimeVisible;
    public bool IsWorldRenderingTimeVisible
    {
        get => _isWorldRenderingTimeVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isWorldRenderingTimeVisible, value));
    }
    
    private TimeSpan _worldRenderingTime;
    public TimeSpan WorldRenderingTime
    {
        get => _worldRenderingTime;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _worldRenderingTime, value));
    }
    #endregion
    #endregion

    private readonly IUiThreadScheduler _uiThreadScheduler;
    
    public StatusBarViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        ISimulationManager simulationManager,
        ISimulationRenderer simulationRenderer,
        IWorldRenderer worldRenderer,
        StatusBarOptions options)
    {
        _uiThreadScheduler = uiThreadScheduler;

        _isCommandProgressVisible = options.IsCommandProgressVisible;
        _isCommandTimeVisible = options.IsCommandTimeVisible;
        _isSimulationRenderingProgressVisible = options.IsSimulationRenderingProgressVisible;
        _isSimulationRenderingTimeVisible = options.IsSimulationRenderingTimeVisible;
        _isWorldRenderingTimeVisible = options.IsWorldRenderingTimeVisible;
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SimulationCommandEventArgs>, SimulationCommandEventArgs>
                (
                    h => simulationManager.CommandStarting += h,
                    h => simulationManager.CommandStarting -= h
                )
                .Where(_ => IsCommandProgressVisible)
                .Select(e => e.EventArgs)
                .Subscribe(e => ExecutingCommand = e.Command)
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => simulationManager.CommandCompleted += h,
                    h => simulationManager.CommandCompleted -= h
                )
                .Do(_ => ExecutingCommand = null)
                .Where(_ => IsCommandTimeVisible)
                .Select(e => e.EventArgs)
                .Subscribe(e => CommandTime = e.ElapsedTime)
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => simulationRenderer.RenderingStarting += h,
                    h => simulationRenderer.RenderingStarting -= h
                )
                .Where(_ => IsSimulationRenderingProgressVisible)
                .Subscribe(_ => IsSimulationRenderingInProgress = true)
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<SimulationRenderingCompletedEventArgs>, SimulationRenderingCompletedEventArgs>
                (
                    h => simulationRenderer.RenderingCompleted += h,
                    h => simulationRenderer.RenderingCompleted -= h
                )
                .Do(_ => IsSimulationRenderingInProgress = false)
                .Where(_ => IsSimulationRenderingTimeVisible)
                .Select(e => e.EventArgs)
                .Subscribe(e => SimulationRenderingTime = e.ElapsedTime)
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<RenderingCompletedEventArgs>, RenderingCompletedEventArgs>
                (
                    h => worldRenderer.RenderingCompleted += h,
                    h => worldRenderer.RenderingCompleted -= h
                )
                .Where(_ => IsWorldRenderingTimeVisible)
                .Select(e => e.EventArgs)
                .Subscribe(e => WorldRenderingTime = e.ElapsedTime)
                .DisposeWith(disposables);
        });
    }
}