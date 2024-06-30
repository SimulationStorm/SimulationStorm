using System;
using DotNext.Threading;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandCompletedEventArgs
(
    SimulationCommand command,
    TimeSpan elapsedTime,
    IAsyncEvent synchronizer
)
    : SimulationCommandEventArgs(command)
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;

    public IAsyncEvent Synchronizer { get; } = synchronizer;
}