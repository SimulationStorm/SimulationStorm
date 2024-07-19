using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Management;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public partial class CellStatesViewModel
(
    GenericCellularAutomationManager gcaManager,
    GenericCellularAutomationSettings settings,
    GenericCellularAutomationOptions options
)
    : ObservableObject
{
    #region Properties
    [ObservableProperty] private bool _randomizeNewCellStateColor;
    #endregion
    
    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddCellState))]
    private async Task AddCellStateAsync()
    {
        var cellState = (byte)0;
        
        var cellStateModel = new CellStateModel
        {
            CellState = cellState,
            Name = "The new cell state", // use localization manager, or create INameCreator service
            Color = RandomizeNewCellStateColor ? ColorUtils.GenerateRandomColor() : default
        };

        await gcaManager
            .ChangeCellStateCollectionAsync(gcaManager.CellStateCollection
                .WithCellState(cellState))
            .ConfigureAwait(false);
        
        settings.CellStateModels.Add(cellStateModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddCellState() => settings.CellStateModels.Count < options.MaxCellStateCount;

    [RelayCommand(CanExecute = nameof(CanRemoveCellState))]
    private async Task RemoveCellStateAsync(CellStateModel cellStateModel)
    {
        var cellState = cellStateModel.CellState;

        await gcaManager
            .ChangeCellStateCollectionAsync(gcaManager.CellStateCollection
                .WithoutCellState(cellState))
            .ConfigureAwait(false);
        
        settings.CellStateModels.Remove(cellStateModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveCellState(CellStateModel cellStateModel) =>
        settings.CellStateModels.Count > 1 && !cellStateModel.IsDefault;

    [RelayCommand(CanExecute = nameof(CanMarkCellStateAsDefault))]
    private async Task MarkCellStateAsDefaultAsync(CellStateModel cellStateModel)
    {
        await gcaManager
            .ChangeCellStateCollectionAsync(gcaManager.CellStateCollection
                .WithDefaultCellState(cellStateModel.CellState))
            .ConfigureAwait(false);
        
        settings.CellStateModels
            .Where(csm => csm != cellStateModel)
            .ForEach(csm => csm.IsDefault = false);
        
        cellStateModel.IsDefault = true;
        
        NotifyCommandsCanExecuteChanged();
    }
    private static bool CanMarkCellStateAsDefault(CellStateModel cellStateModel) => !cellStateModel.IsDefault;
    
    private void NotifyCommandsCanExecuteChanged()
    {
        AddCellStateCommand.NotifyCanExecuteChanged();
        RemoveCellStateCommand.NotifyCanExecuteChanged();
        MarkCellStateAsDefaultCommand.NotifyCanExecuteChanged();
    }
    #endregion
}