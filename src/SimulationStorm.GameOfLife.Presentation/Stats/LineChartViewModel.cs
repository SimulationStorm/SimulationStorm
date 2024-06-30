using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public class LineChartViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    ISummaryStatsManager<GameOfLifeSummary> collectionManager,
    ILocalizationManager localizationManager,
    SummaryStatsOptions options
)
    : CartesianChartViewModelBase(uiThreadScheduler, collectionManager, localizationManager, options)
{
    public IEnumerable<ISeries> Series =>
    [
        new LineSeries<StatsRecordModel>
        {
            Values = RecordModels,
            Mapping = (recordModel, number) => new Coordinate(number, recordModel.AliveCellCount),
            XToolTipLabelFormatter = chartPoint =>
                GetLocalizedString($"Simulation.Command.{chartPoint.Model!.ExecutedCommand.Name}")
        }
    ];
}