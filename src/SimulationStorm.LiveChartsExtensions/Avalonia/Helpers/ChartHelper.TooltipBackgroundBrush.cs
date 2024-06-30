using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.ImageFilters;
using SkiaSharp;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper
{
    public static readonly AttachedProperty<IBrush?> TooltipBackgroundBrushProperty =
        AvaloniaProperty.RegisterAttached<ChartHelper, UserControl, IBrush?>("TooltipBackgroundBrush");

    public static IBrush? GetTooltipBackgroundBrush(UserControl chart) => chart.GetValue(TooltipBackgroundBrushProperty);

    public static void SetTooltipBackgroundBrush(UserControl chart, IBrush? brush)
    {
        ThrowIfNotChart(chart);
        
        chart.SetValue(TooltipBackgroundBrushProperty, brush);
    }
    
    #region Private methods
    private static void HandleTooltipBackgroundBrushChanged(UserControl chart, AvaloniaPropertyChangedEventArgs e)
    {
        var currentTooltipBackgroundPaint = GetChartTooltipBackgroundPaint(chart);
        currentTooltipBackgroundPaint?.Dispose();
        
        if (e.NewValue is not ISolidColorBrush solidColorBrush)
            return;

        var newTooltipBackgroundPaint = new SolidColorPaint
        {
            Color = solidColorBrush.Color.ToSKColor(),
            ImageFilter = new DropShadow(0, 0, 3, 3, SKColors.Black.WithAlpha(255 / 4))
        };
        SetChartTooltipBackgroundPaint(chart, newTooltipBackgroundPaint);
    }

    private static Paint? GetChartTooltipBackgroundPaint(UserControl chart) => chart switch
    {
        PieChart pieChart => pieChart.TooltipBackgroundPaint as Paint,
        PolarChart polarChart => polarChart.TooltipBackgroundPaint as Paint,
        CartesianChart cartesianChart => cartesianChart.TooltipBackgroundPaint as Paint,
        _ => null
    };
    
    private static void SetChartTooltipBackgroundPaint(UserControl chart, Paint paint)
    {
        switch (chart)
        {
            case PieChart pieChart:
            {
                pieChart.TooltipBackgroundPaint = paint;
                break;
            }
            case PolarChart polarChart:
            {
                polarChart.TooltipBackgroundPaint = paint;
                break;
            }
            case CartesianChart cartesianChart:
            {
                cartesianChart.TooltipBackgroundPaint = paint;
                break;
            }
        }
    }
    #endregion
}