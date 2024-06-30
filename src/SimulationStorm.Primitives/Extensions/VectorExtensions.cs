using System.Numerics;

namespace SimulationStorm.Primitives.Extensions;

public static class VectorExtensions
{
    public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);
    
    public static Vector2 ToVector2(this PointF pointF) => new(pointF.X, pointF.Y);
    
    public static Point ToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
    
    public static PointF ToPointF(this Vector2 vector) => new(vector.X, vector.Y);
}