using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation.Presentation.Common;

public interface IBoundedCellularAutomationManagerOptions : IBoundedSimulationManagerOptions
{
    public WorldWrapping WorldWrapping { get; init; }
}