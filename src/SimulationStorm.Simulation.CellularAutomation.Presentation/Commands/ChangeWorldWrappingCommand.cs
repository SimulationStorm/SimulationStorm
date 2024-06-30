using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Commands;

public class ChangeWorldWrappingCommand(WorldWrapping newWorldWrapping) : SimulationCommand("ChangeWorldWrapping", false)
{
    public WorldWrapping NewWorldWrapping { get; } = newWorldWrapping;
}