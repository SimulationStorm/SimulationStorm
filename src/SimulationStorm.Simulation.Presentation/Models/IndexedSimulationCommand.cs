using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Presentation.Models;

public class IndexedSimulationCommand(SimulationCommand command) : IndexedObject
{
    public SimulationCommand Command { get; } = command;
}