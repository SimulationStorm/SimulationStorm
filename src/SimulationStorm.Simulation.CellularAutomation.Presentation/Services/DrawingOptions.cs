using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public class DrawingOptions<TCellState> : IDrawingOptions<TCellState>
{
    public bool IsDrawingEnabled { get; init; }
    
    public DrawingShape BrushShape { get; init; }
    
    public Range<int> BrushRadiusRange { get; init; }
    
    public int BrushRadius { get; init; }

    public TCellState BrushCellState { get; init; } = default!;
}