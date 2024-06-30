using System;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandQueuedEventArgs(SimulationCommand command) : EventArgs
{
    public SimulationCommand Command { get; } = command;
}