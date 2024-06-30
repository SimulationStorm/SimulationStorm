using System;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public class SkiaImageFilter(SKImageFilter skImageFilter) : IImageFilter
{
    public SKImageFilter WrappedSKImageFilter { get; } = skImageFilter;

    public void Dispose()
    {
        WrappedSKImageFilter.Dispose();
        GC.SuppressFinalize(this);
    }
}