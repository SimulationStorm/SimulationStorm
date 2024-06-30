using SimulationStorm.Graphics;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public class BoundedCellularWorldRendererState
{
    public Color BackgroundColor { get; init; }
    
    public bool IsGridLinesVisible { get; init; }

    public Color GridLinesColor { get; init; }
}