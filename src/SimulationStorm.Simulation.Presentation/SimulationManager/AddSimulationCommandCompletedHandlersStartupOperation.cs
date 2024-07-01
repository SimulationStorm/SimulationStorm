using System.Collections.Generic;
using SimulationStorm.Presentation.StartupOperations;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class AddSimulationCommandCompletedHandlersOnStartupOperation
(
    ISimulationManager simulationManager,
    IEnumerable<ISimulationCommandCompletedHandler> commandCompletedHandlers
)
    : IStartupOperation
{
    public void OnStartup()
    {
        foreach (var commandCompletedHandler in commandCompletedHandlers)
            simulationManager.AddCommandCompletedHandler(commandCompletedHandler);
    }
}