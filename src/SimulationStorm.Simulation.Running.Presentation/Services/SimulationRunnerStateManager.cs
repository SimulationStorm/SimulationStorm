using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class SimulationRunnerSaveManager(ISimulationRunner simulationRunner) : ServiceSaveManagerBase<SimulationRunnerSave>
{
    protected override SimulationRunnerSave SaveServiceCore() => new()
    {
        IterationsPerSecondLimit = simulationRunner.MaxStepsPerSecond
    };

    protected override void RestoreServiceSaveCore(SimulationRunnerSave save) =>
        simulationRunner.MaxStepsPerSecond = save.IterationsPerSecondLimit;
}