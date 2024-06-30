using System.Collections.Generic;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Commands;

public class DrawCommand<TCellState>(IEnumerable<Point> cells, TCellState newState) : SimulationCommand("Draw", true)
{
    public IEnumerable<Point> Cells { get; } = cells;

    public TCellState NewState { get; } = newState;
}