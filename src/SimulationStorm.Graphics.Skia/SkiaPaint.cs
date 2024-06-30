using System;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public class SkiaPaint(SKPaint skPaint) : IPaint
{
    public SKPaint WrappedSKPaint { get; } = skPaint;

    public Color Color
    {
        get => WrappedSKPaint.Color.ToColor();
        set => WrappedSKPaint.Color = value.ToSkia();
    }

    public PaintStyle Style
    {
        get => WrappedSKPaint.Style.ToPaintStyle();
        set => WrappedSKPaint.Style = value.ToSkia();
    }

    public float StrokeWidth
    {
        get => WrappedSKPaint.StrokeWidth;
        set => WrappedSKPaint.StrokeWidth = value;
    }

    public IImageFilter ImageFilter
    {
        get => WrappedSKPaint.ImageFilter.ToImageFilter();
        set => WrappedSKPaint.ImageFilter = value.ToSkia();
    }

    public void Dispose()
    {
        WrappedSKPaint.Dispose();
        GC.SuppressFinalize(this);
    }
}