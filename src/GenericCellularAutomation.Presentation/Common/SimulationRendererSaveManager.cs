using SimulationStorm.AppSaves;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;

namespace GenericCellularAutomation.Presentation.Common;

public class SimulationRendererSaveManager(ISimulationRenderer simulationRenderer) :
    ServiceSaveManagerBase<SimulationRendererSave>
{
    protected override SimulationRendererSave SaveServiceCore() => new()
    {
        IsRenderingEnabled = simulationRenderer.IsRenderingEnabled,
        RenderingInterval = simulationRenderer.RenderingInterval,
    };

    protected override void RestoreServiceSaveCore(SimulationRendererSave save)
    {
        simulationRenderer.IsRenderingEnabled = save.IsRenderingEnabled;
        simulationRenderer.RenderingInterval = save.RenderingInterval;
    }
}