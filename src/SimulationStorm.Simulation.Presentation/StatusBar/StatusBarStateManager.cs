using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Presentation.StatusBar;

public class StatusBarSaveManager(StatusBarViewModel statusBarViewModel) : ServiceSaveManagerBase<StatusBarSave>
{
    protected override StatusBarSave SaveServiceCore() => new()
    {
        IsCommandProgressVisible = statusBarViewModel.IsCommandProgressVisible,
        IsCommandTimeVisible = statusBarViewModel.IsCommandTimeVisible,
        IsSimulationRenderingProgressVisible = statusBarViewModel.IsSimulationRenderingProgressVisible,
        IsSimulationRenderingTimeVisible = statusBarViewModel.IsSimulationRenderingTimeVisible,
        IsWorldRenderingTimeVisible = statusBarViewModel.IsWorldRenderingTimeVisible,
        CommandTime = statusBarViewModel.CommandTime,
        SimulationRenderingTime = statusBarViewModel.SimulationRenderingTime,
        WorldRenderingTime = statusBarViewModel.WorldRenderingTime
    };

    protected override void RestoreServiceSaveCore(StatusBarSave save)
    {
        statusBarViewModel.IsCommandProgressVisible = save.IsCommandProgressVisible;
        statusBarViewModel.IsCommandTimeVisible = save.IsCommandTimeVisible;
        statusBarViewModel.IsSimulationRenderingProgressVisible = save.IsSimulationRenderingProgressVisible;
        statusBarViewModel.IsSimulationRenderingTimeVisible = save.IsSimulationRenderingTimeVisible;
        statusBarViewModel.IsWorldRenderingTimeVisible = save.IsWorldRenderingTimeVisible;
        statusBarViewModel.CommandTime = save.CommandTime;
        statusBarViewModel.SimulationRenderingTime = save.SimulationRenderingTime;
        statusBarViewModel.WorldRenderingTime = save.WorldRenderingTime;
    }
}