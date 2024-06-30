using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.Cellular.Presentation.ViewModels;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

public partial class BoundedCellularAutomationWorldViewModel<TAutomationManager, TCellState>
(
    IImmediateUiThreadScheduler uiThreadScheduler,
    IWorldViewport worldViewport,
    IWorldCamera worldCamera,
    IBoundedCellularWorldRenderer worldRenderer,
    TAutomationManager automationManager,
    IDrawingSettings<TCellState> drawingSettings
)
    : BoundedCellularSimulationWorldViewModel(uiThreadScheduler, worldViewport, worldCamera, worldRenderer)
    where TAutomationManager : ICellularAutomationManager<TCellState>, IBoundedSimulationManager
{
    #region Commands
    [RelayCommand(CanExecute = nameof(CanDrawShape))]
    private async Task DrawShapeAtPositions(IEnumerable<Point> positions)
    {
        var automationSizeRect = new Rect(0, 0, automationManager.WorldSize.Width, automationManager.WorldSize.Height);
        var shapesPoints = positions.Select(x => GetShapePoints(x, automationSizeRect)).SelectMany(x => x);
        await automationManager.ChangeCellStatesAsync(shapesPoints, drawingSettings.BrushCellState);
    }
    private bool CanDrawShape() => drawingSettings.IsDrawingEnabled;
    #endregion

    #region Private methods
    private IEnumerable<Point> GetShapePoints(Point center, Rect boundingRect)
    {
        var radius = drawingSettings.BrushRadius;
        if (radius is 0)
            return new[] { center };

        var shapePointsGetter = GetShapePointsGetter(drawingSettings.BrushShape);
        var shapePoints = shapePointsGetter(center, drawingSettings.BrushRadius);

        return shapePoints.Where(boundingRect.Contains);
    }

    private static Func<Point, int, IEnumerable<Point>> GetShapePointsGetter(DrawingShape drawingShape) =>
        drawingShape switch
    {
        DrawingShape.Square => GetSquarePoints,
        DrawingShape.Circle => GetCirclePoints,
        DrawingShape.Triangle => GetTrianglePoints,
        _ => throw new InvalidEnumArgumentException(nameof(drawingShape), (int)drawingShape, typeof(DrawingShape))
    };

    private static IEnumerable<Point> GetSquarePoints(Point center, int radius)
    {
        int diameter = radius * 2,
            startX = center.X - radius,
            startY = center.Y - radius,
            endX = startX + diameter,
            endY = startY + diameter;

        for (var x = startX; x <= endX; x++)
            for (var y = startY; y <= endY; y++)
                yield return new(x, y);
    }

    private static IEnumerable<Point> GetCirclePoints(Point center, int radius)
    {
        int x = radius,
            y = 0,
            radiusError = 1 - x;

        while (x >= y)
        {
            for (var i = -x; i <= x; i++)
            {
                yield return new Point(center.X + i, center.Y + y);
                yield return new Point(center.X + i, center.Y - y);
            }

            for (var i = -y; i <= y; i++)
            {
                yield return new Point(center.X + i, center.Y + x);
                yield return new Point(center.X + i, center.Y - x);
            }

            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
    }

    #region Triangle
    private static IEnumerable<Point> GetTrianglePoints(Point circleCenter, int circleRadius)
    {
        var triangleHeight = circleRadius * Math.Sqrt(3);

        Point topVertex = new(circleCenter.X, circleCenter.Y - circleRadius),
              leftVertex = new(circleCenter.X - circleRadius, circleCenter.Y + (int)(triangleHeight / 2)),
              rightVertex = new(circleCenter.X + circleRadius, circleCenter.Y + (int)(triangleHeight / 2));

        int minX = Math.Min(topVertex.X, Math.Min(leftVertex.X, rightVertex.X)),
            maxX = Math.Max(topVertex.X, Math.Max(leftVertex.X, rightVertex.X)),
            minY = Math.Min(topVertex.Y, Math.Min(leftVertex.Y, rightVertex.Y)),
            maxY = Math.Max(topVertex.Y, Math.Max(leftVertex.Y, rightVertex.Y));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var currentPoint = new Point(x, y);
                if (IsPointInsideTriangle(currentPoint, topVertex, leftVertex, rightVertex))
                    yield return currentPoint;
            }
        }
    }

    private static bool IsPointInsideTriangle(Point point, Point v1, Point v2, Point v3)
    {
        int d1 = GetTriangleAreaSign(point, v1, v2),
            d2 = GetTriangleAreaSign(point, v2, v3),
            d3 = GetTriangleAreaSign(point, v3, v1);

        bool hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0),
             hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNegative && hasPositive);
    }

    private static int GetTriangleAreaSign(Point p1, Point p2, Point p3) =>
        (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
    #endregion
    #endregion
}