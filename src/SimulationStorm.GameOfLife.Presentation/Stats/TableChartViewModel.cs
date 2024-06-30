using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public class TableChartViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    ISummaryStatsManager<GameOfLifeSummary> summaryStatsManager,
    ILocalizationManager localizationManager
)
    : GameOfLifeChartViewModelBase(uiThreadScheduler, summaryStatsManager, localizationManager);