using System;
using System.Collections.Generic;
using System.Linq;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public class DrawingSettingsViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    IDrawingSettings<GameOfLifeCellState> drawingSettings,
    IDrawingOptions<GameOfLifeCellState> options
)
    : DrawingSettingsViewModelBase<GameOfLifeCellState>(uiThreadScheduler, drawingSettings, options)
{
    public override IEnumerable<object> BrushCellStates => Enum.GetValues<GameOfLifeCellState>().Cast<object>();
}