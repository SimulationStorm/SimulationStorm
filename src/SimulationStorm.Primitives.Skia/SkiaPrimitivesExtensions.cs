using SkiaSharp;

namespace SimulationStorm.Primitives.Skia;

public static class SkiaPrimitivesExtensions
{
    public static SKPoint ToSkia(this PointF pointF) => new(pointF.X, pointF.Y);
    public static SKPointI ToSkia(this Point point) => new(point.X, point.Y);
    
    public static SKSize ToSkia(this SizeF sizeF) => new(sizeF.Width, sizeF.Height);
    public static SKSizeI ToSkia(this Size size) => new(size.Width, size.Height);

    public static SKRect ToSkia(this RectF rectF) => new(rectF.Left, rectF.Top, rectF.Right, rectF.Bottom);
    public static SKRectI ToSkia(this Rect rect) => new(rect.Left, rect.Top, rect.Right, rect.Bottom);
}