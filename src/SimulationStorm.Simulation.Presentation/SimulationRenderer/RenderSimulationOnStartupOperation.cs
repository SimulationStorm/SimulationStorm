using SimulationStorm.Presentation.StartupOperations;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public class RenderSimulationOnStartupOperation(ISimulationRenderer simulationRenderer) : IStartupOperation
{
    public void OnStartup() => simulationRenderer.RequestRerender();
}