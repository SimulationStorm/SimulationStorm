using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public abstract class StatsViewModelSaveManagerBase<TSave>
(
    IStatsViewModel statsViewModel
)
    : ServiceSaveManagerBase<TSave> where TSave : StatsViewModelSaveBase, new()
{
    protected override TSave SaveServiceCore() => new()
    {
        SelectedChartType = statsViewModel.CurrentChartType
    };

    protected override void RestoreServiceSaveCore(TSave save) =>
        statsViewModel.CurrentChartType = save.SelectedChartType;
}