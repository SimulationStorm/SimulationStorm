using System.Threading.Tasks;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public interface IAdvanceableSimulationManager : ISimulationManager
{
    Task AdvanceAsync();
}