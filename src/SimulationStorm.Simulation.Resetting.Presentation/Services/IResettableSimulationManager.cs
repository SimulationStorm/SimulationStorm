using System.Threading.Tasks;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Resetting.Presentation.Services;

public interface IResettableSimulationManager : ISimulationManager
{
    Task ResetAsync();
}