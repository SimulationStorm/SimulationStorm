using System;
using SimulationStorm.Primitives;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public class SkiaBitmap(SKBitmap skBitmap) : IBitmap
{
    #region Properties
    public SKBitmap WrappedSkBitmap { get; } = skBitmap;
    
    public Size Size { get; } = new(skBitmap.Width, skBitmap.Height);

    public ColorFormat ColorFormat { get; } = skBitmap.ColorType.ToColorType();

    public PixelAlphaType PixelAlphaType { get; } = skBitmap.AlphaType.ToAlphaType();

    public IntPtr FirstPixelAddress { get; } = skBitmap.GetPixels();

    public int BytesPerRow { get; } = skBitmap.RowBytes;
    #endregion
    
    public void Dispose()
    {
        WrappedSkBitmap.Dispose();
        GC.SuppressFinalize(this);
    }

    public IBitmap Copy() => new SkiaBitmap(WrappedSkBitmap.Copy());
}