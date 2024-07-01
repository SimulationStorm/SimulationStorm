using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Running.Presentation.Models;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Running.Presentation.ViewModels;

public partial class SimulationRunnerViewModel : DisposableObservableObject
{
    #region Properties
    public int MaxStepsPerSecond
    {
        get => _simulationRunner.MaxStepsPerSecond;
        set => _simulationRunner.MaxStepsPerSecond = value;
    }
    
    public SimulationRunningState SimulationRunningState => _simulationRunner.SimulationRunningState;

    [ObservableProperty] private TimeSpan _stepExecutionTime;

    public Range<int> MaxStepsPerSecondRange => _options.MaxStepsPerSecondRange;
    #endregion
    
    #region Commands
    [RelayCommand(CanExecute = nameof(CanAdvanceSimulation))]
    private Task AdvanceSimulationAsync() => _simulationManager.AdvanceAsync();
    private bool CanAdvanceSimulation() => _simulationRunner.SimulationRunningState is SimulationRunningState.Paused;
    
    
    [RelayCommand(CanExecute = nameof(CanResetMaxStepsPerSecond))]
    private void ResetMaxStepsPerSecond() => MaxStepsPerSecond = _options.MaxStepsPerSecond;
    private bool CanResetMaxStepsPerSecond() => MaxStepsPerSecond != _options.MaxStepsPerSecond;

    
    [RelayCommand]
    private void ToggleSimulationRunning()
    {
        if (SimulationRunningState is SimulationRunningState.Paused)
            _simulationRunner.StartSimulation();
        else
            _simulationRunner.PauseSimulation();
    }
    #endregion
    
    #region Fields
    private readonly IAdvanceableSimulationManager _simulationManager;

    private readonly ISimulationRunner _simulationRunner;
    
    private readonly SimulationRunnerOptions _options;
    #endregion

    public SimulationRunnerViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        IAdvanceableSimulationManager simulationManager,
        ISimulationRunner simulationRunner,
        SimulationRunnerOptions options)
    {
        _simulationManager = simulationManager;
        _simulationRunner = simulationRunner;
        _options = options;
        
        _simulationRunner
            .WhenValueChanged(x => x.SimulationRunningState, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ =>
            {
                OnPropertyChanged(nameof(SimulationRunningState));
                AdvanceSimulationCommand.NotifyCanExecuteChanged();
            })
            .DisposeWith(Disposables);
        
        _simulationRunner
            .WhenValueChanged(x => x.MaxStepsPerSecond, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ =>
            {
                OnPropertyChanged(nameof(MaxStepsPerSecond));
                ResetMaxStepsPerSecondCommand.NotifyCanExecuteChanged();
            })
            .DisposeWith(Disposables);

        Observable
            .FromEventPattern<EventHandler<SimulationAdvancedEventArgs>, SimulationAdvancedEventArgs>
            (
                h => _simulationRunner.SimulationAdvanced += h,
                h => _simulationRunner.SimulationAdvanced -= h
            )
            .Select(ep => ep.EventArgs)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(e => StepExecutionTime = e.ElapsedTime)
            .DisposeWith(Disposables);
    }
}