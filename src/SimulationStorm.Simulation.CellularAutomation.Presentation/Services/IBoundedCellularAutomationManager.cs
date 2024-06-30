using System.Threading.Tasks;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public interface IBoundedCellularAutomationManager<in TCellState> : ICellularAutomationManager<TCellState>
{
    WorldWrapping WorldWrapping { get; }
    
    Task ChangeWorldWrappingAsync(WorldWrapping newWrapping);
}