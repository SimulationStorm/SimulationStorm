using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.ViewModels;

public class RenderingStatsViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    IRenderingStatsManager renderingStatsManager,
    RenderingChartViewModelFactory chartViewModelFactory,
    IUiThemeManager uiThemeManager,
    RenderingStatsOptions options
)
    : StatsViewModelBase<RenderingResultRecord>(
        uiThreadScheduler, renderingStatsManager, chartViewModelFactory, uiThemeManager, options);