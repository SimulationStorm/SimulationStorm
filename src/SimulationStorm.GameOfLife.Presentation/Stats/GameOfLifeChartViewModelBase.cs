using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public abstract class GameOfLifeChartViewModelBase
(
    IUiThreadScheduler uiThreadScheduler,
    ISummaryStatsManager<GameOfLifeSummary> summaryStatsManager,
    ILocalizationManager localizationManager
)
    : SummaryStatsChartViewModelBase<GameOfLifeSummary, StatsRecordModel>(
        uiThreadScheduler, summaryStatsManager, localizationManager)
{
    protected override StatsRecordModel CreateRecordModel(SummaryRecord<GameOfLifeSummary> record) =>
        new(record.ExecutedCommand, record.SummarizingTime, record.Summary.DeadCellCount, record.Summary.AliveCellCount);
}