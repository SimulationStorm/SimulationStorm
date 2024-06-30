using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Commands;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

public partial class WorldWrappingViewModel<TCellState> : DisposableObservableObject, IWorldWrappingViewModel
{
    #region Properties
    public WorldWrapping ActualWorldWrapping => _automationManager.WorldWrapping;
    
    public bool IsWrappedHorizontally
    {
        get => _editingWorldWrapping is WorldWrapping.Horizontal or WorldWrapping.Both;
        set
        {
            _editingWorldWrapping = value switch
            {
                true when _editingWorldWrapping is WorldWrapping.NoWrap => WorldWrapping.Horizontal,
                true when _editingWorldWrapping is WorldWrapping.Vertical => WorldWrapping.Both,
                true => _editingWorldWrapping,
        
                false when _editingWorldWrapping is WorldWrapping.Horizontal => WorldWrapping.NoWrap,
                false when _editingWorldWrapping is WorldWrapping.Both => WorldWrapping.Vertical,
                false => _editingWorldWrapping
            };
            
            OnPropertyChanged();
            ChangeWorldWrappingCommand.NotifyCanExecuteChanged();
        }
    }

    public bool IsWrappedVertically
    {
        get => _editingWorldWrapping is WorldWrapping.Vertical or WorldWrapping.Both;
        set
        {
            _editingWorldWrapping = value switch
            {
                true when _editingWorldWrapping is WorldWrapping.NoWrap => WorldWrapping.Vertical,
                true when _editingWorldWrapping is WorldWrapping.Horizontal => WorldWrapping.Both,
                true => _editingWorldWrapping,

                false when _editingWorldWrapping is WorldWrapping.Vertical => WorldWrapping.NoWrap,
                false when _editingWorldWrapping is WorldWrapping.Both => WorldWrapping.Horizontal,
                false => _editingWorldWrapping
            };
            
            OnPropertyChanged();
            ChangeWorldWrappingCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    [RelayCommand(CanExecute = nameof(CanChangeWorldWrapping))]
    private async Task ChangeWorldWrapping()
    {
        await _automationManager.ChangeWorldWrappingAsync(_editingWorldWrapping);
        
        OnPropertyChanged(nameof(ActualWorldWrapping));
        ChangeWorldWrappingCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeWorldWrapping() => _editingWorldWrapping != _automationManager.WorldWrapping;

    #region Fields
    private readonly IBoundedCellularAutomationManager<TCellState> _automationManager;

    private WorldWrapping _editingWorldWrapping;
    #endregion

    public WorldWrappingViewModel(IUiThreadScheduler uiThreadScheduler, IBoundedCellularAutomationManager<TCellState> automationManager)
    {
        _automationManager = automationManager;
        _editingWorldWrapping = automationManager.WorldWrapping;
        
        WithDisposables(disposables =>
        {
            var executedCommandStream = Observable
                .FromEventPattern<EventHandler<SimulationCommandExecutedEventArgs>, SimulationCommandExecutedEventArgs>
                (
                    h => _automationManager.CommandExecuted += h,
                    h => _automationManager.CommandExecuted -= h
                )
                .Select(e => e.EventArgs.Command)
                .ObserveOn(uiThreadScheduler);
                
            executedCommandStream
                .Where(command => command is ChangeWorldWrappingCommand)
                .Subscribe(_ => OnPropertyChanged(nameof(ActualWorldWrapping)))
                .DisposeWith(disposables);
            
            executedCommandStream
                .Where(command => command is RestoreStateCommand)
                .Subscribe(_ =>
                {
                    if (_editingWorldWrapping == ActualWorldWrapping)
                        return;

                    OnPropertyChanged(nameof(ActualWorldWrapping));
                    
                    _editingWorldWrapping = ActualWorldWrapping;
                    OnPropertyChanged(nameof(IsWrappedHorizontally));
                    OnPropertyChanged(nameof(IsWrappedVertically));
                })
                .DisposeWith(disposables);
        });
    }
}