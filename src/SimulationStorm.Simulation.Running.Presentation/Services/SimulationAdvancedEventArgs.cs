using System;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class SimulationAdvancedEventArgs(TimeSpan elapsedTime) : EventArgs
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
}