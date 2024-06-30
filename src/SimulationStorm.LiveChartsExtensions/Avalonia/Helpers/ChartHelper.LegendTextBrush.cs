using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;

namespace SimulationStorm.LiveChartsExtensions.Avalonia.Helpers;

public partial class ChartHelper
{
    public static readonly AttachedProperty<IBrush?> LegendTextBrushProperty =
        AvaloniaProperty.RegisterAttached<ChartHelper, UserControl, IBrush?>("LegendTextBrush");

    public static IBrush? GetLegendTextBrush(UserControl chart) => chart.GetValue(LegendTextBrushProperty);

    public static void SetLegendTextBrush(UserControl chart, IBrush? brush)
    {
        ThrowIfNotChart(chart);
            
        chart.SetValue(LegendTextBrushProperty, brush);
    }

    #region Private methods
    private static void HandleLegendTextBrushChanged(UserControl chart, AvaloniaPropertyChangedEventArgs e)
    {
        var currentLegendTextPaint = GetChartLegendTextPaint(chart);
        currentLegendTextPaint?.Dispose();
        
        if (e.NewValue is not ISolidColorBrush solidColorBrush)
            return;
        
        var newLegendTextPaint = new SolidColorPaint(solidColorBrush.Color.ToSKColor());
        SetChartLegendTextPaint(chart, newLegendTextPaint);
    }

    private static Paint? GetChartLegendTextPaint(UserControl chart) => chart switch
    {
        PieChart pieChart => pieChart.LegendTextPaint as Paint,
        PolarChart polarChart => polarChart.LegendTextPaint as Paint,
        CartesianChart cartesianChart => cartesianChart.LegendTextPaint as Paint,
        _ => null
    };
    
    private static void SetChartLegendTextPaint(UserControl chart, Paint paint)
    {
        switch (chart)
        {
            case PieChart pieChart:
            {
                pieChart.LegendTextPaint = paint;
                break;
            }
            case PolarChart polarChart:
            {
                polarChart.LegendTextPaint = paint;
                break;
            }
            case CartesianChart cartesianChart:
            {
                cartesianChart.LegendTextPaint = paint;
                break;
            }
        }
    }
    #endregion
}