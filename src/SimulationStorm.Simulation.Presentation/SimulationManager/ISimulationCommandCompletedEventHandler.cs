using System;
using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public interface ISimulationCommandCompletedEventHandler
{
    Task HandleCommandCompletedAsync(SimulationCommand command, TimeSpan elapsedTime);
}