using SimulationStorm.Graphics;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public class CellularWorldRendererOptions : ICellularWorldRendererOptions
{
    public bool IsGridLinesVisible { get; init; }
    
    public Color GridLinesColor { get; init; }
    
    public Color HoveredCellColor { get; init; }

    public Color PressedCellColor { get; init; }
}