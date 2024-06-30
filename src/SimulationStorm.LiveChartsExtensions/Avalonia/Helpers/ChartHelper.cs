using System;
using Avalonia;
using Avalonia.Controls;
using LiveChartsCore.SkiaSharpView.Avalonia;
using SimulationStorm.LiveChartsExtensions.ThemeManagement;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper : AvaloniaObject
{
    static ChartHelper()
    {
        LegendTextBrushProperty.Changed.AddClassHandler<UserControl>(HandleLegendTextBrushChanged);
        LegendBackgroundBrushProperty.Changed.AddClassHandler<UserControl>(HandleLegendBackgroundBrushChanged);
        TooltipTextBrushProperty.Changed.AddClassHandler<UserControl>(HandleTooltipTextBrushChanged);
        TooltipBackgroundBrushProperty.Changed.AddClassHandler<UserControl>(HandleTooltipBackgroundBrushChanged);
        
        ObserveThemeChangesProperty.Changed.AddClassHandler<UserControl>(HandleObserveThemeChangesChanged);
        LvcThemeManager.Instance.ThemeChanged += HandleLiveChartsThemeChanged;
    }
    
    protected static void ThrowIfNotChartOrMap(UserControl chart)
    {
        if (!IsChartOrMap(chart))
            throw new InvalidOperationException($"The {nameof(ChartHelper)} properties can only be attached to a chart or map control.");
    }
    
    protected static void ThrowIfNotChart(UserControl chart)
    {
        if (!IsChart(chart))
            throw new InvalidOperationException($"The {nameof(ChartHelper)} properties can only be attached to a chart control.");
    }

    protected static bool IsChartOrMap(UserControl userControl) => IsChart(userControl) || userControl is GeoMap;

    protected static bool IsChart(UserControl userControl) => userControl switch
    {
        PieChart => true,
        PolarChart => true,
        CartesianChart => true,
        _ => false
    };
}