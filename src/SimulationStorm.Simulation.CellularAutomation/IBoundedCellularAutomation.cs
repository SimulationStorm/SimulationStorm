using SimulationStorm.Simulation.Bounded;

namespace SimulationStorm.Simulation.CellularAutomation;

public interface IBoundedCellularAutomation<TCellState> : ICellularAutomation<TCellState>, IBoundedSimulation
{
    WorldWrapping WorldWrapping { get; set; }
}