using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Presentation.StatusBar;

public class StatusBarStateManager(StatusBarViewModel statusBarViewModel) : ServiceStateManagerBase<StatusBarState>
{
    protected override StatusBarState SaveServiceStateImpl() => new()
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

    protected override void RestoreServiceStateImpl(StatusBarState state)
    {
        statusBarViewModel.IsCommandProgressVisible = state.IsCommandProgressVisible;
        statusBarViewModel.IsCommandTimeVisible = state.IsCommandTimeVisible;
        statusBarViewModel.IsSimulationRenderingProgressVisible = state.IsSimulationRenderingProgressVisible;
        statusBarViewModel.IsSimulationRenderingTimeVisible = state.IsSimulationRenderingTimeVisible;
        statusBarViewModel.IsWorldRenderingTimeVisible = state.IsWorldRenderingTimeVisible;
        statusBarViewModel.CommandTime = state.CommandTime;
        statusBarViewModel.SimulationRenderingTime = state.SimulationRenderingTime;
        statusBarViewModel.WorldRenderingTime = state.WorldRenderingTime;
    }
}