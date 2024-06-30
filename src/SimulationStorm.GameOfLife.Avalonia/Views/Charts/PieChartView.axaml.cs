using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.GameOfLife.Presentation.Stats;

namespace SimulationStorm.GameOfLife.Avalonia.Views.Charts;

public partial class PieChartView : UserControl
{
    public PieChartView() => InitializeComponent();

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is not PieChartViewModel viewModel)
            return;
        
        // [WORKAROUND] This is needed to update chart legend colors...
        this.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => viewModel.InvalidationRequested += h,
                    h => viewModel.InvalidationRequested -= h
                )
                .Subscribe(_ =>
                {
                    ChartControl.IsVisible = false;
                    ChartControl.IsVisible = true;
                })
                .DisposeWith(disposables);
        });
    }
}