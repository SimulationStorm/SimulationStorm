using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public abstract class StatsViewModelStateManagerBase<TState>
(
    IStatsViewModel statsViewModel
)
    : ServiceStateManagerBase<TState> where TState : StatsViewModelStateBase, new()
{
    protected override TState SaveServiceStateImpl() => new()
    {
        SelectedChartType = statsViewModel.CurrentChartType
    };

    protected override void RestoreServiceStateImpl(TState state) =>
        statsViewModel.CurrentChartType = state.SelectedChartType;
}