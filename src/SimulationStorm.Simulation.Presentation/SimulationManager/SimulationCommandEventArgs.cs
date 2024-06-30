using System;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandEventArgs(SimulationCommand command) : EventArgs
{
    public SimulationCommand Command { get; } = command;
}