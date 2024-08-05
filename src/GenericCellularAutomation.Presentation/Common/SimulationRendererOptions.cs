using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;

namespace GenericCellularAutomation.Presentation.Common;

public class SimulationRendererOptions : ISimulationRendererOptions
{
    public bool IsRenderingEnabled { get; init; }
    
    public Range<int> RenderingIntervalRange { get; init; }
    
    public int RenderingInterval { get; init; }
}