using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Patterns;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public class GameOfLifeDrawingSettingsState : DrawingSettingsState<GameOfLifeCellState>
{
    public NamedPattern? Pattern { get; set; }
    
    public bool PlacePatternWithOverlay { get; set; }
}