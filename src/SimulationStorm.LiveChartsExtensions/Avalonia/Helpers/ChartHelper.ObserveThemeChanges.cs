using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using DotNext.Collections.Generic;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper
{
    public static readonly AttachedProperty<bool> ObserveThemeChangesProperty =
        AvaloniaProperty.RegisterAttached<ChartHelper, UserControl, bool>("ObserveThemeChanges");

    private static readonly IList<WeakReference<UserControl>> SubscribedChartReferences =
        new List<WeakReference<UserControl>>();

    public static bool GetObserveThemeChanges(UserControl chart) =>
        chart.GetValue(ObserveThemeChangesProperty);
    
    public static void SetObserveThemeChanges(UserControl chart, bool observeThemeChanges)
    {
        ThrowIfNotChartOrMap(chart);
        
        chart.SetValue(ObserveThemeChangesProperty, observeThemeChanges);
    }

    private static void HandleObserveThemeChangesChanged(UserControl chart, AvaloniaPropertyChangedEventArgs e)
    {
        var observeThemeChanges = e.GetNewValue<bool>();

        RemoveEmptyChartReferences();
        var chartReferenceExists = TryGetChartReference(chart, out var chartReference);
        
        if (observeThemeChanges && !chartReferenceExists)
            SubscribedChartReferences.Add(new WeakReference<UserControl>(chart));
        else if (!observeThemeChanges && chartReferenceExists)
            SubscribedChartReferences.Remove(chartReference!);
    }

    private static void HandleLiveChartsThemeChanged(object? sender, EventArgs e)
    {
        RemoveEmptyChartReferences();
        NotifyChartsAboutThemeChange();
    }

    private static void RemoveEmptyChartReferences() => SubscribedChartReferences
        .Where(chartReference => !chartReference.TryGetTarget(out _))
        .ToList()
        .ForEach(chartReference => SubscribedChartReferences.Remove(chartReference));

    private static bool TryGetChartReference(UserControl chart,
        [NotNullWhen(true)] out WeakReference<UserControl>? chartReference)
    {
        chartReference = null;
        
        foreach (var chartRef in SubscribedChartReferences)
        {
            chartRef.TryGetTarget(out var otherChart);
            if (otherChart != chart)
                continue;
            
            chartReference = chartRef;
            break;
        }

        return chartReference is not null;
    }

    private static void NotifyChartsAboutThemeChange() => SubscribedChartReferences.ForEach(chartReference =>
    {
        chartReference.TryGetTarget(out var chart);
        chart!.InvalidateVisual();
    });
}