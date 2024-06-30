using System;
using System.ComponentModel;
using System.Numerics;
using Avalonia;
using Avalonia.Platform;
using AvaloniaColor = Avalonia.Media.Color;
using AvaloniaVector = Avalonia.Vector;
using AvaloniaPoint = Avalonia.Point;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;

namespace SimulationStorm.Graphics.Avalonia.Extensions;

public static class AvaloniaGraphicsExtensions
{
    #region Conversions to Avalonia types
    public static AvaloniaColor ToAvalonia(this Color color) => new(color.Alpha, color.Red, color.Green, color.Blue);
    
    public static AvaloniaBitmap ToAvalonia(this IBitmap bitmap) => new
    (
        bitmap.ColorFormat.ToAvalonia(),
        bitmap.PixelAlphaType.ToAvalonia(),
        bitmap.FirstPixelAddress,
        new PixelSize(bitmap.Size.Width, bitmap.Size.Height),
        new AvaloniaVector(96, 96),
        bitmap.BytesPerRow
    );
    
    public static AlphaFormat ToAvalonia(this PixelAlphaType pixelAlphaType) => pixelAlphaType switch
    {
        PixelAlphaType.Opaque => AlphaFormat.Opaque,
        PixelAlphaType.Premultiplied => AlphaFormat.Premul,
        PixelAlphaType.Unpremultiplied => AlphaFormat.Unpremul,
        _ => throw new InvalidEnumArgumentException(nameof(pixelAlphaType), (int)pixelAlphaType, typeof(PixelAlphaType))
    };
    
    public static PixelFormat ToAvalonia(this ColorFormat colorFormat) => colorFormat switch
    {
        ColorFormat.Rgba8888 => PixelFormat.Rgba8888,
        ColorFormat.Bgra8888 => PixelFormat.Bgra8888,
        _ => throw new NotSupportedException()
    };
    #endregion

    #region Conversions from Avalonia types
    public static Color ToColor(this AvaloniaColor color) => new(color.R, color.G, color.B, color.A);

    public static Vector2 ToVector2(this AvaloniaPoint point) => new((float)point.X, (float)point.Y);
    #endregion
}