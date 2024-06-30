using Avalonia;
using Avalonia.Input;

namespace SimulationStorm.Avalonia.Extensions;

public static class PointerEventArgsExtensions
{
    public static PointerPoint GetEarliestPoint(this PointerEventArgs pointerEventArgs, Visual? relativeTo) =>
        pointerEventArgs.GetIntermediatePoints(relativeTo)[0];

    public static Vector GetPositionDelta(this PointerEventArgs pointerEventArgs, Visual? relativeTo)
    {
        var intermediatePoints = pointerEventArgs.GetIntermediatePoints(relativeTo);
        var earliestPoint = intermediatePoints[0];
        var currentPoint = intermediatePoints[^1];

        var pointPositionsDelta = currentPoint.Position - earliestPoint.Position;
        return new Vector(pointPositionsDelta.X, pointPositionsDelta.Y);
    }
}