using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class ScheduledCommandsViewModelStateManager(ScheduledCommandsViewModel scheduledCommandsViewModel) :
    ServiceStateManagerBase<ScheduledCommandsViewModelState>
{
    protected override ScheduledCommandsViewModelState SaveServiceStateImpl() => new()
    {
        AreScheduledCommandsVisible = scheduledCommandsViewModel.AreScheduledCommandsVisible
    };

    protected override void RestoreServiceStateImpl(ScheduledCommandsViewModelState state) =>
        scheduledCommandsViewModel.AreScheduledCommandsVisible = state.AreScheduledCommandsVisible;
}