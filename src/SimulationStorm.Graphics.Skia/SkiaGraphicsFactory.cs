using System;
using SimulationStorm.Primitives;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public class SkiaGraphicsFactory : IGraphicsFactory
{
    public IBitmap CreateBitmap(Size size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size.Width, 1, $"{nameof(size)}{nameof(size.Width)}");
        ArgumentOutOfRangeException.ThrowIfLessThan(size.Height, 1, $"{nameof(size)}{nameof(size.Height)}");
            
        return new SkiaBitmap(new SKBitmap(size.Width, size.Height));
    }

    public ICanvas CreateCanvas(IBitmap bitmap) => new SkiaCanvas(new SKCanvas(bitmap.ToSkia()));

    public IPaint CreatePaint() => new SkiaPaint(new SKPaint());

    public IImageFilter CreateDropShadowImageFilter
    (
        float offsetX,
        float offsetY,
        float blurRadiusX,
        float blurRadiusY,
        Color color)
    {
        var skImageFilter = SKImageFilter.CreateDropShadow(offsetX, offsetY, blurRadiusX, blurRadiusY, color.ToSkia());
        return new SkiaImageFilter(skImageFilter);
    }
}