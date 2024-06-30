using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.WorldRenderer;

namespace SimulationStorm.Simulation.Bounded.Presentation.Services;

public interface IBoundedWorldRenderer : IWorldRenderer
{
    Color DefaultBackgroundColor { get; }
    
    Color BackgroundColor { get; set; }
    
    Rect SimulationImageRect { get; }
}