using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.Themes;
using SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement;

public static class LvcThemeExtensions
{
    public static Theme<SkiaSharpDrawingContext> WithColors
    (
        this Theme<SkiaSharpDrawingContext> theme,
        LvcColor[] colors)
    {
        theme.Colors = colors;
        return theme;
    }
    
    public static Theme<SkiaSharpDrawingContext> ConfigureRules
    (
        this Theme<SkiaSharpDrawingContext> theme,
        LvcThemeOptions options)
    {
        return theme
            .ConfigureRuleForAxis(options.AxisOptions)
            .ConfigureRuleForAnySeries()
            .ConfigureRuleForLineSeries(options.LineSeriesOptions)
            .ConfigureRuleForBarSeries(options.BarSeriesOptions)
            .ConfigureRuleForPieSeries();
    }

    public static Theme<SkiaSharpDrawingContext> ConfigureRuleForAxis
    (
        this Theme<SkiaSharpDrawingContext> theme,
        LvcAxisOptions options
    )
        => theme.HasRuleForAxes(axis =>
        {
            axis.NamePaint = LvcHelpers.CreateTextPaint(options.NameFont, options.NameColor);
            axis.NameTextSize = options.NameFontSize;

            axis.LabelsPaint = LvcHelpers.CreateTextPaint(options.LabelsFont, options.LabelsColor);
            axis.TextSize = options.LabelsFontSize;

            axis.ShowSeparatorLines = options.AreSeparatorLinesVisible;
            
            if (axis is ICartesianAxis cartesianAxis)
            {
                cartesianAxis.Padding = new Padding(9);
        
                cartesianAxis.NamePadding = cartesianAxis.Orientation switch
                {
                    AxisOrientation.X => LvcStaticData.EmptyPadding,
                    AxisOrientation.Y => new Padding(0, 0, 0, 3),
                    _ => cartesianAxis.NamePadding
                };
                
                axis.SeparatorsPaint = cartesianAxis.Orientation is AxisOrientation.Y
                    ? LvcHelpers.CreateFillPaint(options.SeparatorsColor)
                    : null;
            }
            else
            {
                axis.NamePadding = LvcStaticData.EmptyPadding;
                axis.SeparatorsPaint = options.AreSeparatorLinesVisible
                    ? LvcHelpers.CreateFillPaint(options.SeparatorsColor)
                    : null;
            }
        });
    
    public static Theme<SkiaSharpDrawingContext> ConfigureRuleForAnySeries
    (
        this Theme<SkiaSharpDrawingContext> theme
    )
        => theme.HasRuleForAnySeries(series => series.Name = LiveCharts.IgnoreSeriesName);
    
    public static Theme<SkiaSharpDrawingContext> ConfigureRuleForLineSeries
    (
        this Theme<SkiaSharpDrawingContext> theme,
        LvcLineSeriesOptions options
    )
        => theme.HasRuleForLineSeries(lineSeries =>
        {
            var seriesColor = theme.GetSeriesColor(lineSeries);

            lineSeries.Stroke = LvcHelpers.CreateFillPaint(seriesColor);
            lineSeries.Stroke.StrokeThickness = options.StrokeSize;
            lineSeries.Fill = LvcHelpers.CreateFillPaint(seriesColor.WithOpacity(255 / 4));

            lineSeries.GeometrySize = options.GeometryOptions.Size;
            lineSeries.GeometryStroke = LvcHelpers.CreateFillPaint(seriesColor);
            lineSeries.GeometryStroke.StrokeThickness = options.GeometryOptions.StrokeSize;
            lineSeries.GeometryFill = LvcHelpers.CreateFillPaint(options.GeometryOptions.FillColor);
        });
    
    public static Theme<SkiaSharpDrawingContext> ConfigureRuleForBarSeries
    (
        this Theme<SkiaSharpDrawingContext> theme,
        LvcBarSeriesOptions options
    )
        => theme.HasRuleForBarSeries(barSeries =>
        {
            var seriesColor = theme.GetSeriesColor(barSeries);

            barSeries.Stroke = null;
            barSeries.Fill = LvcHelpers.CreateFillPaint(seriesColor);
            barSeries.Rx = options.CornerRadiusOnXAxis;
            barSeries.Ry = options.CornerRadiusOnYAxis;
        });

    public static Theme<SkiaSharpDrawingContext> ConfigureRuleForPieSeries
    (
        this Theme<SkiaSharpDrawingContext> theme
    )
        => theme.HasRuleForPieSeries(pieSeries =>
        {
            var seriesColor = theme.GetSeriesColor(pieSeries);

            pieSeries.Stroke = null;
            pieSeries.Fill = LvcHelpers.CreateFillPaint(seriesColor);
        });
}