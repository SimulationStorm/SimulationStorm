using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Rendering;
using SimulationStorm.Graphics.Skia;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;
using SkiaSharp;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public sealed class PieChartViewModel : GameOfLifeChartViewModelBase
{
    public IEnumerable<ISeries> Series => [_deadCellsSeries, _aliveCellsSeries];

    public event EventHandler? InvalidationRequested;

    #region Fields
    private readonly ILocalizationManager _localizationManager;
    
    private readonly GameOfLifeRenderer _gameOfLifeRenderer;

    private readonly SummaryStatsOptions _options;
    
    private readonly PieSeries<ObservableValue> _deadCellsSeries,
                                                _aliveCellsSeries;
    
    private readonly ObservableValue _deadCellCount = new(0),
                                     _aliveCellCount = new(0);

    private readonly Paint _deadCellsSeriesFillPaint = new SolidColorPaint(),
                           _deadCellsSeriesLabelPaint = new SolidColorPaint(),
                           _aliveCellsSeriesFillPaint = new SolidColorPaint(),
                           _aliveCellsSeriesLabelPaint = new SolidColorPaint();
    #endregion

    public PieChartViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        ISummaryStatsManager<GameOfLifeSummary> summaryStatsManager,
        ILocalizationManager localizationManager,
        GameOfLifeRenderer gameOfLifeRenderer,
        SummaryStatsOptions options
    )
        : base(uiThreadScheduler, summaryStatsManager, localizationManager)
    {
        _localizationManager = localizationManager;
        _gameOfLifeRenderer = gameOfLifeRenderer;
        _options = options;
        
        _deadCellsSeries = CreatePieSeries(_deadCellCount, _deadCellsSeriesFillPaint, _deadCellsSeriesLabelPaint);
        _aliveCellsSeries = CreatePieSeries(_aliveCellCount, _aliveCellsSeriesFillPaint, _aliveCellsSeriesLabelPaint);
        
        RecordModels
            .ObserveCollectionChanges()
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => UpdateCellCountsByLastRecordModel())
            .DisposeWith(Disposables);
        
        gameOfLifeRenderer
            .WhenValueChanged(x => x.CellColors)
            .Subscribe(_ =>
            {
                UpdateCellPaintColors();
                
                InvalidationRequested?.Invoke(this, EventArgs.Empty);
            })
            .DisposeWith(Disposables);

        Disposables.AddRange(_deadCellsSeriesFillPaint, _deadCellsSeriesLabelPaint,
            _aliveCellsSeriesFillPaint, _aliveCellsSeriesLabelPaint);
        
        UpdateCellCountsByLastRecordModel();
        UpdateSeriesNames();
    }

    protected override void OnCultureChanged() => UpdateSeriesNames();

    #region Private methods
    private static PieSeries<ObservableValue> CreatePieSeries(ObservableValue cellCountValue, Paint fillPaint, Paint labelPaint) => new()
    {
        Values = new[] { cellCountValue },
        Fill = fillPaint,
        DataLabelsPaint = labelPaint,
        DataLabelsSize = 16,
        DataLabelsFormatter = chartPoint =>
        {
            var percent = chartPoint.StackedValue!.Share;
            return percent > 0 ? $"{percent:P0}" : string.Empty;
        },
        ToolTipLabelFormatter = chartPoint => $"{chartPoint.Model!.Value:N0}"
    };

    private void UpdateCellCountsByLastRecordModel()
    {
        if (RecordModels.Count is 0)
        {
            _deadCellCount.Value = 0;
            _aliveCellCount.Value = 0;
            return;
        }

        var lastRecordModel = RecordModels[^1];
        _deadCellCount.Value = lastRecordModel.DeadCellCount;
        _aliveCellCount.Value = lastRecordModel.AliveCellCount;
    }

    private void UpdateCellPaintColors()
    {
        SKColor deadCellColor = _gameOfLifeRenderer.CellColors.DeadCellColor.ToSkia(),
            aliveCellColor = _gameOfLifeRenderer.CellColors.AliveCellColor.ToSkia();
        
        _deadCellsSeriesFillPaint.Color = deadCellColor;
        _deadCellsSeriesLabelPaint.Color = aliveCellColor;
        
        _aliveCellsSeriesFillPaint.Color = aliveCellColor;
        _aliveCellsSeriesLabelPaint.Color = deadCellColor;
    }

    private void UpdateSeriesNames()
    {
        _deadCellsSeries.Name = _localizationManager.GetLocalizedString(_options.DeadCellsStringKey);
        _aliveCellsSeries.Name = _localizationManager.GetLocalizedString(_options.AliveCellsStringKey);
    }
    #endregion
}