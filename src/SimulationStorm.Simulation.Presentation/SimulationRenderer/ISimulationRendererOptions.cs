using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public interface ISimulationRendererOptions
{
    public bool IsRenderingEnabled { get; }

    public Range<int> RenderingIntervalRange { get; }
    
    public int RenderingInterval { get; }
}