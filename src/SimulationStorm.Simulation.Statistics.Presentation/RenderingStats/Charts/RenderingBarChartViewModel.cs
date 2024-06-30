using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Presentation.TimeFormatting;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;

public class RenderingBarChartViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    IRenderingStatsManager renderingStatsManager,
    ILocalizationManager localizationManager,
    ITimeFormatter timeFormatter,
    RenderingStatsOptions options
)
    : RenderingCartesianChartViewModelBase(uiThreadScheduler,
        renderingStatsManager, localizationManager, timeFormatter, options)
{
    public ISeries[] Series =>
    [
        new ColumnSeries<RenderingResultRecordModel>
        {
            Values = RecordModels,
            Mapping = (record, _) => new Coordinate(record.OrdinalNumber, record.RenderingTime.Ticks),
            XToolTipLabelFormatter = chartPoint => StringifyRecordModel(chartPoint.Model!)
        }
    ];
}