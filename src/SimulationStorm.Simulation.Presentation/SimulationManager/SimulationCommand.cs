namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommand(string name, bool changesWorld)
{
    public string Name { get; } = name;

    public bool ChangesWorld { get; } = changesWorld;
}