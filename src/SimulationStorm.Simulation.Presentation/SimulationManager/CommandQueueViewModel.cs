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

public partial class CommandQueueViewModel : DisposableObservableObject
{
    #region Properties
    public ReadOnlyObservableCollection<IndexedSimulationCommand> CommandQueue => _commandQueue;

    private bool _isCommandQueueVisible;
    public bool IsCommandQueueVisible
    {
        get => _isCommandQueueVisible;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _isCommandQueueVisible, value));
    }

    [ObservableProperty] private bool _areThereCommands;
    #endregion

    #region Fields
    private readonly IUiThreadScheduler _uiThreadScheduler;
    
    private ReadOnlyObservableCollection<IndexedSimulationCommand> _commandQueue = null!;
    #endregion
    
    public CommandQueueViewModel(IUiThreadScheduler uiThreadScheduler, ISimulationManager simulationManager)
    {
        _uiThreadScheduler = uiThreadScheduler;
        
        WithDisposables(disposables =>
        {
            simulationManager.ScheduledCommandQueue
                .IndexItemsAndBind<ReadOnlyObservableCollection<SimulationCommand>, SimulationCommand, IndexedSimulationCommand>
                (
                    command => new IndexedSimulationCommand(command),
                    out _commandQueue,
                    scheduler: uiThreadScheduler
                )
                .DisposeWith(disposables);

            simulationManager.ScheduledCommandQueue
                .WhenValueChanged(x => x.Count)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(commandCount => AreThereCommands = commandCount is not 0)
                .DisposeWith(disposables);
        });
    }
}