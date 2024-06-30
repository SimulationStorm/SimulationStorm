using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper
{
    public static readonly AttachedProperty<IBrush?> LegendBackgroundBrushProperty =
        AvaloniaProperty.RegisterAttached<ChartHelper, UserControl, IBrush?>("LegendBackgroundBrush");

    public static IBrush? GetLegendBackgroundBrush(UserControl chart) =>
        chart.GetValue(LegendBackgroundBrushProperty);

    public static void SetLegendBackgroundBrush(UserControl chart, IBrush? brush)
    {
        ThrowIfNotChart(chart);
            
        chart.SetValue(LegendBackgroundBrushProperty, brush);
    }

    #region Private methods
    private static void HandleLegendBackgroundBrushChanged(UserControl chart, AvaloniaPropertyChangedEventArgs e)
    {
        var currentLegendBackgroundPaint = GetChartLegendBackgroundPaint(chart);
        currentLegendBackgroundPaint?.Dispose();
        
        if (e.NewValue is not ISolidColorBrush solidColorBrush)
            return;

        var newLegendBackgroundPaint = new SolidColorPaint(solidColorBrush.Color.ToSKColor());
        SetChartLegendBackgroundPaint(chart, newLegendBackgroundPaint);
    }

    private static Paint? GetChartLegendBackgroundPaint(UserControl chart) => chart switch
    {
        PieChart pieChart => pieChart.LegendBackgroundPaint as Paint,
        PolarChart polarChart => polarChart.LegendBackgroundPaint as Paint,
        CartesianChart cartesianChart => cartesianChart.LegendBackgroundPaint as Paint,
        _ => null
    };
    
    private static void SetChartLegendBackgroundPaint(UserControl chart, Paint paint)
    {
        switch (chart)
        {
            case PieChart pieChart:
            {
                pieChart.LegendBackgroundPaint = paint;
                break;
            }
            case PolarChart polarChart:
            {
                polarChart.LegendBackgroundPaint = paint;
                break;
            }
            case CartesianChart cartesianChart:
            {
                cartesianChart.LegendBackgroundPaint = paint;
                break;
            }
        }
    }
    #endregion
}