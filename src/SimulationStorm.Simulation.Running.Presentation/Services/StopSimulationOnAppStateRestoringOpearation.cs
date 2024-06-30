using SimulationStorm.AppStates.Operations;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class PauseSimulationOnAppStateRestoringOperation(ISimulationRunner simulationRunner) :
    IAppStateRestoringOperation
{
    public void OnAppStateRestoring() => simulationRunner.PauseSimulation();
}