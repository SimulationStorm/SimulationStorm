using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class ScheduledCommandsViewModelSaveManager(ScheduledCommandsViewModel scheduledCommandsViewModel) :
    ServiceSaveManagerBase<ScheduledCommandsViewModelSave>
{
    protected override ScheduledCommandsViewModelSave SaveServiceCore() => new()
    {
        AreScheduledCommandsVisible = scheduledCommandsViewModel.AreScheduledCommandsVisible
    };

    protected override void RestoreServiceSaveCore(ScheduledCommandsViewModelSave save) =>
        scheduledCommandsViewModel.AreScheduledCommandsVisible = save.AreScheduledCommandsVisible;
}