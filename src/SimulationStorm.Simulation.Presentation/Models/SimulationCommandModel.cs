using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Presentation.Models;

public class SimulationCommandModel(SimulationCommand command) : IndexedObject
{
    public SimulationCommand Command { get; } = command;
}