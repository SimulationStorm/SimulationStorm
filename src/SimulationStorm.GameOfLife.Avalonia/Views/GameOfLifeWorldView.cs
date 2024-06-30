using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.GameOfLife.Presentation.ViewModels;
using SimulationStorm.Simulation.Cellular.Avalonia.Views;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public class GameOfLifeWorldView : BoundedCellularSimulationWorldView
{
    private readonly GameOfLifeWorldViewModel _viewModel;
    
    public GameOfLifeWorldView()
    {
        _viewModel = DiContainer.Default.GetRequiredService<GameOfLifeWorldViewModel>();
        Initialize(_viewModel);
    }

    protected override void OnCellReleased()
    {
        base.OnCellReleased();

        // Todo: World view base should be refactored to avoid this condition...
        if (PressedCellsHistory.Count is 0)
            return;

        var previousPressedCells = ConnectPreviousPressedCellsToLinesAndGetPoints().ToArray();

        if (_viewModel.PlacePatternAtPositionsCommand.CanExecute(previousPressedCells))
            _viewModel.PlacePatternAtPositionsCommand.Execute(previousPressedCells);
        else if (_viewModel.DrawShapeAtPositionsCommand.CanExecute(previousPressedCells))
            _viewModel.DrawShapeAtPositionsCommand.Execute(previousPressedCells);
            
        PressedCellsHistory.Clear();
    }
}