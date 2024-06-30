using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public interface IDrawingOptions<out TCellState>
{
    bool IsDrawingEnabled { get; }
    
    DrawingShape BrushShape { get; }

    Range<int> BrushRadiusRange { get; }
    
    int BrushRadius { get; }
    
    TCellState BrushCellState { get; }
}