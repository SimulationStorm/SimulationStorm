using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Bounded.Presentation.Commands;

public class ChangeWorldSizeCommand(Size newSize) : SimulationCommand("ChangeWorldSize", true)
{
    public Size NewSize { get; } = newSize;
}