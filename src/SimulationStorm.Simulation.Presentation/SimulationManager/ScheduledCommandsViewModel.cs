using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using SimulationStorm.Simulation.Presentation.Models;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public partial class ScheduledCommandsViewModel : DisposableObservableObject
{
    #region Properties
    public ReadOnlyObservableCollection<SimulationCommandModel> ScheduledCommandModels { get; }

    private bool _areScheduledCommandsVisible;
    public bool AreScheduledCommandsVisible
    {
        get => _areScheduledCommandsVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _areScheduledCommandsVisible, value));
    }

    [ObservableProperty] private bool _areThereCommands;
    #endregion

    #region Fields
    private readonly IUiThreadScheduler _uiThreadScheduler;
    #endregion
    
    public ScheduledCommandsViewModel(IUiThreadScheduler uiThreadScheduler, ISimulationManager simulationManager)
    {
        _uiThreadScheduler = uiThreadScheduler;

        ReadOnlyObservableCollection<SimulationCommandModel> scheduledCommandModels = null!;
        
        WithDisposables(disposables =>
        {
            var scheduledCommands = new ObservableCollection<SimulationCommand>();

            simulationManager
                .CommandSchedulingObservable()
                .Subscribe(e => scheduledCommands.Add(e.Command));

            simulationManager
                .CommandCompletedObservable()
                .Subscribe(_ => scheduledCommands.RemoveAt(0));
            
            scheduledCommands
                .IndexItemsAndBind<ObservableCollection<SimulationCommand>, SimulationCommand, SimulationCommandModel>
                (
                    command => new SimulationCommandModel(command),
                    out scheduledCommandModels,
                    scheduler: uiThreadScheduler
                )
                .DisposeWith(disposables);

            scheduledCommands
                .WhenValueChanged(x => x.Count)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(commandCount => AreThereCommands = commandCount is not 0)
                .DisposeWith(disposables);
        });

        ScheduledCommandModels = scheduledCommandModels;
    }
}