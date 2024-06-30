using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class CommandQueueViewModelStateManager(CommandQueueViewModel commandQueueViewModel) :
    ServiceStateManagerBase<CommandQueueViewModelState>
{
    protected override CommandQueueViewModelState SaveServiceStateImpl() => new()
    {
        IsCommandQueueVisible = commandQueueViewModel.IsCommandQueueVisible
    };

    protected override void RestoreServiceStateImpl(CommandQueueViewModelState state) =>
        commandQueueViewModel.IsCommandQueueVisible = state.IsCommandQueueVisible;
}