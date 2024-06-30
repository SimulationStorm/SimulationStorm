using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public interface ICellularAutomationManager<in TCellState> : ISimulationManager
{
    Task ChangeCellStatesAsync(IEnumerable<Point> cells, TCellState newState);
}