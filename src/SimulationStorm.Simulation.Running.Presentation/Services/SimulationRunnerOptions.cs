using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class SimulationRunnerOptions
{
    public Range<int> MaxStepsPerSecondRange { get; init; }
    
    public int MaxStepsPerSecond { get; init; }
}