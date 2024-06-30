using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Drawing;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.ViewModels;

public partial class GameOfLifeWorldViewModel
(
    IImmediateUiThreadScheduler uiThreadScheduler,
    IWorldViewport worldViewport,
    IWorldCamera worldCamera,
    IBoundedCellularWorldRenderer worldRenderer,
    GameOfLifeManager gameOfLifeManager,
    GameOfLifeDrawingSettings drawingSettings
)
    : BoundedCellularAutomationWorldViewModel<GameOfLifeManager, GameOfLifeCellState>
    (uiThreadScheduler, worldViewport, worldCamera, worldRenderer, gameOfLifeManager, drawingSettings)
{
    [RelayCommand(CanExecute = nameof(CanPlacePattern))]
    private Task PlacePatternAtPositions(IEnumerable<Point> positions) =>
        gameOfLifeManager.PlacePatternAtPositionsAsync(
            drawingSettings.CurrentPattern!.Pattern, positions, drawingSettings.PlacePatternWithOverlay);
    
    private bool CanPlacePattern() => drawingSettings.CurrentPattern is not null;
}