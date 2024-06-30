using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LiveChartsCore.SkiaSharpView;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.LiveChartsExtensions.Axes;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public abstract class CartesianChartViewModelBase : GameOfLifeChartViewModelBase
{
    #region Properties

    public IEnumerable<Axis> XAxes => [_commandNumberAxis];

    public IEnumerable<Axis> YAxes => [_aliveCellCountAxis];
    #endregion

    #region Fields
    private readonly SummaryStatsOptions _options;
    
    private readonly AxisExtended _commandNumberAxis,
                                  _aliveCellCountAxis;
    #endregion
    
    protected CartesianChartViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        ISummaryStatsManager<GameOfLifeSummary> summaryStatsManager,
        ILocalizationManager localizationManager,
        SummaryStatsOptions options
    )
        : base(uiThreadScheduler, summaryStatsManager, localizationManager)
    {
        _options = options;
        
        _commandNumberAxis = new AxisExtended
        {
            MinStep = 1,
            Labeler = ordinalNumber => $"{ordinalNumber + 1}"
        };
        
        _aliveCellCountAxis = new AxisExtended
        {
            Labeler = cellCount => $"{cellCount:N0}",
            MinStep = 1
        };
        
        UpdateAxisNames();
    }

    protected override void OnCultureChanged() => UpdateAxisNames();

    private void UpdateAxisNames()
    {
        _commandNumberAxis.Name = GetLocalizedString(_options.CommandNumberAxisNameKey);
        _aliveCellCountAxis.Name = GetLocalizedString(_options.AliveCellCountAxisNameKey);
    }
}