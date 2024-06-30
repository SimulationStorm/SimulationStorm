using SimulationStorm.Graphics;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public interface ICellularWorldRendererOptions
{
    bool IsGridLinesVisible { get; }

    Color GridLinesColor { get; }
    
    Color HoveredCellColor { get; }
    
    Color PressedCellColor { get; }
}