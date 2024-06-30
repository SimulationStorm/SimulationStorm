using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Patterns;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public class GameOfLifeDrawingOptions : DrawingOptions<GameOfLifeCellState>
{
    public NamedPattern? Pattern { get; init; }
    
    public bool PlacePatternWithOverlay { get; init; }
}