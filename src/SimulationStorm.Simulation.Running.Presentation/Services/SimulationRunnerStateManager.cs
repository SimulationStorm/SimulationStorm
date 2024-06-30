using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public class SimulationRunnerStateManager(ISimulationRunner simulationRunner) : ServiceStateManagerBase<SimulationRunnerState>
{
    protected override SimulationRunnerState SaveServiceStateImpl() => new()
    {
        IterationsPerSecondLimit = simulationRunner.MaxStepsPerSecond
    };

    protected override void RestoreServiceStateImpl(SimulationRunnerState state) =>
        simulationRunner.MaxStepsPerSecond = state.IterationsPerSecondLimit;
}