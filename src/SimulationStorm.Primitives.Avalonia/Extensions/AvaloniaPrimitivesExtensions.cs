using System.Numerics;
using AvaloniaPoint = Avalonia.Point;
using AvaloniaVector = Avalonia.Vector;
using AvaloniaSize = Avalonia.Size;

namespace SimulationStorm.Primitives.Avalonia.Extensions;

public static class AvaloniaPrimitivesExtensions
{
    public static PointF ToPointF(this AvaloniaPoint avaloniaPoint) =>
        new((float)avaloniaPoint.X, (float)avaloniaPoint.Y);
    
    public static Point ToPoint(this AvaloniaPoint avaloniaPoint) =>
        new((int)avaloniaPoint.X, (int)avaloniaPoint.Y);

    public static Vector2 ToVector2(this AvaloniaVector vector) =>
        new((float)vector.X, (float)vector.Y);

    public static Size ToSize(this AvaloniaSize avaloniaSize) =>
        new((int)avaloniaSize.Width, (int)avaloniaSize.Height);
    
    public static SizeF ToSizeF(this AvaloniaSize avaloniaSize) =>
        new((float)avaloniaSize.Width, (float)avaloniaSize.Height);
}