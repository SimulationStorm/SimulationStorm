using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Bounded.Presentation.Services;

public interface IBoundedSimulationManagerOptions : ISimulationManagerOptions
{
    Size WorldSize { get; }
    
    Range<Size> WorldSizeRange { get; }
}