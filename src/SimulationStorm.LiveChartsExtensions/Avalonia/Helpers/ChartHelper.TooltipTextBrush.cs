using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper
{
    public static readonly AttachedProperty<IBrush?> TooltipTextBrushProperty =
        AvaloniaProperty.RegisterAttached<ChartHelper, UserControl, IBrush?>("TooltipTextBrush");

    public static IBrush? GetTooltipTextBrush(UserControl chart) => chart.GetValue(TooltipTextBrushProperty);

    public static void SetTooltipTextBrush(UserControl chart, IBrush? brush)
    {
        ThrowIfNotChart(chart);
        
        chart.SetValue(TooltipTextBrushProperty, brush);
    }
    
    #region Private methods
    private static void HandleTooltipTextBrushChanged(UserControl chart, AvaloniaPropertyChangedEventArgs e)
    {
        var currentTooltipTextPaint = GetChartTooltipTextPaint(chart);
        currentTooltipTextPaint?.Dispose();
        
        if (e.NewValue is not ISolidColorBrush solidColorBrush)
            return;

        var newTooltipTextPaint = new SolidColorPaint(solidColorBrush.Color.ToSKColor());
        SetChartTooltipTextPaint(chart, newTooltipTextPaint);
    }

    private static Paint? GetChartTooltipTextPaint(UserControl chart) => chart switch
    {
        PieChart pieChart => pieChart.TooltipTextPaint as Paint,
        PolarChart polarChart => polarChart.TooltipTextPaint as Paint,
        CartesianChart cartesianChart => cartesianChart.TooltipTextPaint as Paint,
        _ => null
    };
    
    private static void SetChartTooltipTextPaint(UserControl chart, Paint paint)
    {
        switch (chart)
        {
            case PieChart pieChart:
            {
                pieChart.TooltipTextPaint = paint;
                break;
            }
            case PolarChart polarChart:
            {
                polarChart.TooltipTextPaint = paint;
                break;
            }
            case CartesianChart cartesianChart:
            {
                cartesianChart.TooltipTextPaint = paint;
                break;
            }
        }
    }
    #endregion
}