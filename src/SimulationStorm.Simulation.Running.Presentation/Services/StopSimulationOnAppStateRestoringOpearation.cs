using SimulationStorm.AppSaves.Operations;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class PauseSimulationOnAppSaveRestoringOperation(ISimulationRunner simulationRunner) :
    IAppSaveRestoringOperation
{
    public void OnAppSaveRestoring() => simulationRunner.PauseSimulation();
}