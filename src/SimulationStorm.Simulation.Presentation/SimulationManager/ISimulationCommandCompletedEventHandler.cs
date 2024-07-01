using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public interface ISimulationCommandCompletedHandler
{
    Task HandleSimulationCommandCompletedAsync(SimulationCommandCompletedEventArgs e);
}