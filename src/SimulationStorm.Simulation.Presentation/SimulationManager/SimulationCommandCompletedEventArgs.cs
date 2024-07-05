using System;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandCompletedEventArgs
(
    SimulationCommand command,
    TimeSpan elapsedTime
)
    : SimulationCommandEventArgs(command)
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
}