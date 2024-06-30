using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DynamicData.Binding;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Statistics.Presentation;

namespace SimulationStorm.Simulation.Statistics.Avalonia;

public static class StatsViewExtensions
{
    public static void BindChartViewModelToContentControl
    (
        this Control control,
        IStatsViewModel viewModel,
        ContentControl chartContentControl)
    {
        control.WithDisposables(disposables =>
        {
            viewModel
                .WhenValueChanged(x => x.CurrentChartViewModel)
                .Subscribe(chartViewModel =>
                {
                    chartContentControl.Content = null;
                    chartContentControl.Content = chartViewModel;
                })
                .DisposeWith(disposables);
        });
    }
}