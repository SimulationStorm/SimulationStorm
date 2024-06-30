using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;

public class SummaryStatsViewModel<TSummary>
(
    IUiThreadScheduler uiThreadScheduler,
    ISummaryStatsManager<TSummary> collectionManager,
    SummaryStatsChartViewModelFactory chartViewModelFactory,
    IUiThemeManager uiThemeManager,
    ISummaryStatsOptions options
)
    : StatsViewModelBase<SummaryRecord<TSummary>>(uiThreadScheduler,
        collectionManager, chartViewModelFactory, uiThemeManager, options), ISummaryStatsViewModel;