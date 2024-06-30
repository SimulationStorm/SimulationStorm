using LiveChartsCore.SkiaSharpView;
using SimulationStorm.LiveChartsExtensions.Axes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Presentation.TimeFormatting;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;

public abstract class CommandExecutionCartesianChartViewModelBase :
    ChartViewModelBase<CommandExecutionResultRecord, CommandExecutionResultRecordModel>
{
    #region Properties
    public Axis[] XAxes => [_commandNumberAxis];

    public Axis[] YAxes => [_executionTimeAxis];
    #endregion

    #region Fields
    private readonly CommandExecutionStatsOptions _options;
    
    private readonly AxisExtended _commandNumberAxis;
    
    private readonly TimeSpanAxisExtended _executionTimeAxis;
    #endregion
    
    protected CommandExecutionCartesianChartViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        ICommandExecutionStatsManager commandExecutionStatsManager,
        ILocalizationManager localizationManager,
        ITimeFormatter timeFormatter,
        CommandExecutionStatsOptions options
    )
        : base(uiThreadScheduler, commandExecutionStatsManager, localizationManager)
    {
        _options = options;
        
        _commandNumberAxis = new AxisExtended
        {
            MinStep = 1,
            Labeler = ordinalNumber => $"{ordinalNumber + 1}"
        };

        _executionTimeAxis = new TimeSpanAxisExtended(_options.ExecutionTimeAxisTimeUnit, timeFormatter.FormatTime);
        
        UpdateAxisNames();
    }

    protected override CommandExecutionResultRecordModel CreateRecordModel(
        CommandExecutionResultRecord record) => new(record.ExecutedCommand, record.CommandExecutionTime);
    
    protected override void OnCultureChanged() => UpdateAxisNames();

    private void UpdateAxisNames()
    {
        _commandNumberAxis.Name = GetLocalizedString(_options.CommandNumberAxisNameKey);
        _executionTimeAxis.Name = GetLocalizedString(_options.ExecutionTimeAxisNameKey);
    }
}