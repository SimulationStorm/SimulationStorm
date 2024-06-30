using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Presentation.TimeFormatting;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;

public class CommandExecutionBarChartViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    ICommandExecutionStatsManager commandExecutionStatsManager,
    ILocalizationManager localizationManager,
    ITimeFormatter timeFormatter,
    CommandExecutionStatsOptions options
)
    : CommandExecutionCartesianChartViewModelBase(uiThreadScheduler,
        commandExecutionStatsManager, localizationManager, timeFormatter, options)
{
    public ISeries[] Series =>
    [
        new ColumnSeries<CommandExecutionResultRecordModel>
        {
            Values = RecordModels,
            Mapping = (record, _) => new Coordinate(record.OrdinalNumber, record.CommandExecutionTime.Ticks),
            XToolTipLabelFormatter = chartPoint =>
                GetLocalizedString($"Simulation.Command.{chartPoint.Model!.ExecutedCommand.Name}")
        }
    ];
}