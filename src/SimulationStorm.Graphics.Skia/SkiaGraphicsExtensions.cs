using System;
using System.ComponentModel;
using System.Numerics;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public static class SkiaGraphicsExtensions
{
    #region Conversions to Skia types
    public static SKColorType ToSkia(this ColorFormat colorFormat) => colorFormat switch
    {
        ColorFormat.Rgba8888 => SKColorType.Rgba8888,
        ColorFormat.Bgra8888 => SKColorType.Bgra8888,
        _ => throw new NotSupportedException()
    };
    
    public static SKAlphaType ToSkia(this PixelAlphaType pixelAlphaType) => pixelAlphaType switch
    {
        PixelAlphaType.Opaque => SKAlphaType.Opaque,
        PixelAlphaType.Premultiplied => SKAlphaType.Premul,
        PixelAlphaType.Unpremultiplied => SKAlphaType.Unpremul,
        _ => throw new InvalidEnumArgumentException(nameof(pixelAlphaType), (int)pixelAlphaType, typeof(PixelAlphaType))
    };
    
    public static SKPaintStyle ToSkia(this PaintStyle paintStyle) => paintStyle switch
    {
        PaintStyle.Fill => SKPaintStyle.Fill,
        PaintStyle.Stroke => SKPaintStyle.Stroke,
        PaintStyle.FillAndStroke => SKPaintStyle.StrokeAndFill,
        _ => throw new InvalidEnumArgumentException(nameof(paintStyle), (int)paintStyle, typeof(PaintStyle))
    };
    
    public static SKColor ToSkia(this Color color) => new(color.Red, color.Green, color.Blue, color.Alpha);
    
    public static SKMatrix ToSkia(this Matrix3x2 matrix) => new()
    {
        ScaleX = matrix.M11,
        SkewX = matrix.M21,
        TransX = matrix.M31,
        SkewY = matrix.M12,
        ScaleY = matrix.M22,
        TransY = matrix.M32,
        Persp0 = 0,
        Persp1 = 0,
        Persp2 = 1
    };

    public static SKPaint ToSkia(this IPaint paint) => ((SkiaPaint)paint).WrappedSKPaint;

    public static SKBitmap ToSkia(this IBitmap bitmap) => ((SkiaBitmap)bitmap).WrappedSkBitmap;

    public static SKCanvas ToSkia(this ICanvas canvas) => ((SkiaCanvas)canvas).WrappedSkCanvas;

    public static SKImageFilter ToSkia(this IImageFilter imageFilter) => ((SkiaImageFilter)imageFilter).WrappedSKImageFilter;
    #endregion

    #region Conversion from Skia types
    public static ColorFormat ToColorType(this SKColorType skColorType) => skColorType switch
    {
        SKColorType.Rgba8888 => ColorFormat.Rgba8888,
        SKColorType.Bgra8888 => ColorFormat.Bgra8888,
        _ => throw new NotSupportedException()
    };
    
    public static PixelAlphaType ToAlphaType(this SKAlphaType skAlphaType) => skAlphaType switch
    {
        SKAlphaType.Opaque => PixelAlphaType.Opaque,
        SKAlphaType.Premul => PixelAlphaType.Premultiplied,
        SKAlphaType.Unpremul => PixelAlphaType.Unpremultiplied,
        _ => throw new InvalidEnumArgumentException(nameof(skAlphaType), (int)skAlphaType, typeof(SKAlphaType))
    };

    public static PaintStyle ToPaintStyle(this SKPaintStyle skPaintStyle) => skPaintStyle switch
    {
        SKPaintStyle.Fill => PaintStyle.Fill,
        SKPaintStyle.Stroke => PaintStyle.Stroke,
        SKPaintStyle.StrokeAndFill => PaintStyle.FillAndStroke,
        _ => throw new InvalidEnumArgumentException(nameof(skPaintStyle), (int)skPaintStyle, typeof(SKPaintStyle))
    };
    
    public static Color ToColor(this SKColor skColor) => new(skColor.Red, skColor.Green, skColor.Blue, skColor.Alpha);

    public static IImageFilter ToImageFilter(this SKImageFilter skImageFilter) => new SkiaImageFilter(skImageFilter);

    public static ICanvas ToCanvas(this SKCanvas skCanvas) => new SkiaCanvas(skCanvas);
    #endregion
}