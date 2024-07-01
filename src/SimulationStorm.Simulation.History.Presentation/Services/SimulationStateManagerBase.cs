using System.Threading.Tasks;
using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class SimulationSaveManagerBase<TSave>(ISaveableSimulationManager<TSave> simulationManager) :
    AsyncServiceSaveManagerBase<TSave>
{
    protected override Task<TSave> SaveServiceAsyncCore() => simulationManager.SaveAsync();

    protected override Task RestoreServiceSaveAsyncCore(TSave save)
    {
        // await simulationManager.ClearCommandQueueAsync();
        return simulationManager.RestoreStateAsync(save, true);
    }
}