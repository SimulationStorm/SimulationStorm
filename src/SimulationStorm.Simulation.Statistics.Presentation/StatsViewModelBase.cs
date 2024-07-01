using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public abstract partial class StatsViewModelBase<TRecord> : CollectionManagerViewModelBase<TRecord>, IStatsViewModel
{
    #region Properties
    public IEnumerable<ChartType> ChartTypes { get; }
    
    private ChartType _currentChartType;
    public ChartType CurrentChartType
    {
        get => _currentChartType;
        set
        {
            if (SetProperty(ref _currentChartType, value))
                UpdateChartViewModel();
        }
    }

    private IDisposable? _currentChartViewModel;
    public IDisposable? CurrentChartViewModel
    {
        get => _currentChartViewModel;
        set => _uiThreadScheduler.Schedule(() => SetProperty(ref _currentChartViewModel, value));
    }
    #endregion

    #region Fields
    private readonly IUiThreadScheduler _uiThreadScheduler;
    
    private readonly IChartViewModelFactory _chartViewModelFactory;
    #endregion

    protected StatsViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        ICollectionManager<TRecord> collectionManager,
        IChartViewModelFactory chartViewModelFactory,
        IUiThemeManager uiThemeManager,
        IStatsOptions options
    )
        : base(uiThreadScheduler, collectionManager, options)
    {
        _uiThreadScheduler = uiThreadScheduler;
        _chartViewModelFactory = chartViewModelFactory;
        
        ChartTypes = options.ChartTypes;
        _currentChartType = options.DefaultChartType;
        
        // [WORKAROUND] This is needed to update chart tooltip background color.
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => uiThemeManager.ThemeChanged += h,
                h => uiThemeManager.ThemeChanged -= h
            )
            .Subscribe(_ => UpdateChartViewModel())
            .DisposeWith(Disposables);

        collectionManager
            .WhenValueChanged(x => x.IsSavingEnabled, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => UpdateChartViewModel())
            .DisposeWith(Disposables);
    }

    public override void Dispose()
    {
        base.Dispose();
        CurrentChartViewModel?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void UpdateChartViewModel()
    {
        if (!IsSavingEnabled)
            return;
        
        CurrentChartViewModel?.Dispose();
        CurrentChartViewModel = _chartViewModelFactory.CreateChartViewModel(CurrentChartType);
    }
}