using System;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandExecutingEventArgs(SimulationCommand command) : EventArgs
{
    public SimulationCommand Command { get; } = command;
}