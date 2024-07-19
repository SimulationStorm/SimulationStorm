using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericCellularAutomation.Presentation.Management;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public partial class CellStatesViewModel : ObservableObject
{
    #region Properties
    [ObservableProperty] private bool _randomizeNewCellStateColor;
    #endregion
    
    #region Fields
    private readonly GenericCellularAutomationManager _gcaManager;

    private readonly GenericCellularAutomationSettings _settings;
    #endregion
    
    public CellStatesViewModel
    (
        GenericCellularAutomationManager gcaManager,
        GenericCellularAutomationSettings settings)
    {
        _gcaManager = gcaManager;
        _settings = settings;
    }
    
    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddCellState))]
    private void AddCellState()
    {
        var cellState = (byte)0;
        
        var cellStateModel = new CellStateModel
        {
            CellState = cellState,
            Name = "The new cell state", // use localization manager, or create INameCreator service
            Color = RandomizeNewCellStateColor ? ColorUtils.GenerateRandomColor() : default
        };

        _gcaManager.PossibleCellStateCollection =
            _gcaManager.PossibleCellStateCollection.WithCellState(cellState);
        
        _settings.CellStateModels.Add(cellStateModel);
    }
    private bool CanAddCellState() => true;

    [RelayCommand(CanExecute = nameof(CanRemoveCellState))]
    private void RemoveCellState(CellStateModel cellStateModel)
    {
        var cellState = cellStateModel.CellState;

        _gcaManager.PossibleCellStateCollection =
            _gcaManager.PossibleCellStateCollection.WithoutCellState(cellState);
        
        _settings.CellStateModels.Remove(cellStateModel);
    }
    private bool CanRemoveCellState() => true;
    #endregion
}