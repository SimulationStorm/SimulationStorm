using LiveChartsCore.SkiaSharpView;
using SimulationStorm.LiveChartsExtensions.Axes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Presentation.TimeFormatting;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;

public abstract class RenderingCartesianChartViewModelBase :
    ChartViewModelBase<RenderingResultRecord, RenderingResultRecordModel>
{
    #region Public properties
    public Axis[] XAxes => [_commandNumberAxis];

    public Axis[] YAxes => [_renderingTimeAxis];
    #endregion
    
    #region Fields
    private readonly RenderingStatsOptions _options;
    
    private readonly AxisExtended _commandNumberAxis;
    
    private readonly TimeSpanAxisExtended _renderingTimeAxis;
    #endregion
    
    protected RenderingCartesianChartViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        IRenderingStatsManager renderingStatsManager,
        ILocalizationManager localizationManager,
        ITimeFormatter timeFormatter,
        RenderingStatsOptions options
    )
        : base(uiThreadScheduler, renderingStatsManager, localizationManager)
    {
        _options = options;
        
        _commandNumberAxis = new AxisExtended
        {
            MinStep = 1,
            Labeler = ordinalNumber => $"{ordinalNumber + 1}"
        };

        _renderingTimeAxis = new TimeSpanAxisExtended(_options.RenderingTimeAxisTimeUnit, timeFormatter.FormatTime);
        
        UpdateAxisNames();
    }

    protected override RenderingResultRecordModel CreateRecordModel(RenderingResultRecord record) =>
        new(record.ExecutedCommand, record.RenderingTime);

    protected override void OnCultureChanged() => UpdateAxisNames();

    protected string StringifyRecordModel(RenderingResultRecordModel recordModel)
    {
        if (recordModel.ExecutedCommand is { } executedCommand)
            return GetLocalizedString($"Simulation.Command.{executedCommand.Name}");

        return GetLocalizedString("Simulation.Rendering.ChangeRenderingSettings");
    }

    private void UpdateAxisNames()
    {
        _commandNumberAxis.Name = GetLocalizedString(_options.CommandNumberAxisNameKey);
        _renderingTimeAxis.Name = GetLocalizedString(_options.RenderingTimeAxisNameKey);
    }
}