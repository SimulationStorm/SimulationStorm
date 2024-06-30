using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.ViewModels;

public class CommandExecutionStatsViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    ICommandExecutionStatsManager speedTimeMeasurer,
    CommandExecutionChartViewModelFactory chartViewModelFactory,
    IUiThemeManager uiThemeManager,
    CommandExecutionStatsOptions options
)
    : StatsViewModelBase<CommandExecutionResultRecord>(
        uiThreadScheduler, speedTimeMeasurer, chartViewModelFactory, uiThemeManager, options);