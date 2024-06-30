using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.CellularAutomation;

public interface ICellularAutomation<TCellState> : ISimulation
{
    TCellState GetCellState(Point cell);

    void SetCellState(Point cell, TCellState newState);
}