using System.Threading.Tasks;
using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class SimulationStateManagerBase<TState>(ISaveableSimulationManager<TState> simulationManager) :
    AsyncServiceStateManagerBase<TState>
{
    protected override Task<TState> SaveServiceStateAsyncImpl() => simulationManager.SaveAsync();

    protected override Task RestoreServiceStateAsyncImpl(TState state)
    {
        // await simulationManager.ClearCommandQueueAsync();
        return simulationManager.RestoreStateAsync(state, true);
    }
}