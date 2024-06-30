using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SimulationStorm.LiveChartsExtensions.FontManagement;
using SkiaSharp;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement;

public static class LvcHelpers
{
    public static Paint CreateTextPaint(LvcFont font, LvcColor color) => new SolidColorPaint
    {
        SKTypeface = LvcFontManager.Instance.GetTypeface(font),
        Color = color.AsSKColor()
    };

    public static Paint CreateFillPaint(LvcColor color) => new SolidColorPaint
    {
        Color = color.AsSKColor(),
        Style = SKPaintStyle.Fill
    };
}