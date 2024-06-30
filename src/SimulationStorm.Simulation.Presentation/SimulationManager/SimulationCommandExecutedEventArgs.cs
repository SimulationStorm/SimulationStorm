using System;
using DotNext.Threading;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandExecutedEventArgs
(
    SimulationCommand command,
    TimeSpan elapsedTime,
    IAsyncEvent synchronizer
)
    : EventArgs
{
    public SimulationCommand Command { get; } = command;

    public TimeSpan ElapsedTime { get; } = elapsedTime;

    public IAsyncEvent Synchronizer { get; } = synchronizer;
}