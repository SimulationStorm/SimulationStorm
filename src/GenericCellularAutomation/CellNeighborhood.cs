using System;
using System.Collections.Generic;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation;

public sealed class CellNeighborhood
{
    #region Constants
    private const int MinRadius = 1;

    private const int MinNeighborCount = 0;
    #endregion

    #region Properties
    public int Radius { get; }
    
    public IReadOnlySet<Point> UsedPositions { get; }

    public int Side => GetSideByRadius(Radius);

    public Size Size => new(Side, Side);

    public int MaxNeighborCount => GetMaxNeighborCellCountWithinRadius(Radius);
    #endregion

    public CellNeighborhood(int radius, IReadOnlySet<Point> usedPositions)
    {
        ValidateRadius(radius);
        Radius = radius;
        
        ValidatePositionsWithinRadius(radius, usedPositions);
        UsedPositions = usedPositions;
    }

    #region Static methods
    // The assumption: all methods take a valid radius
    public static int GetSideByRadius(int radius) => radius * 2 + 1;
    
    public static bool IsCenterPosition(Point position) => position.X is 0 && position.Y is 0;
    
    public static int GetMaxPositionCountWithinRadius(int radius)
    {
        var side = GetSideByRadius(radius);
        return side * side - 1; // -1 to exclude center
    }

    public static void ValidateRadius(int radius) =>
        ArgumentOutOfRangeException.ThrowIfLessThan(radius, MinRadius, nameof(radius));

    #region Position related methods
    public static void ForEachPositionWithinRadius(int radius, Action<Point> action)
    {
        foreach (var position in GetAllPositionsWithinRadius(radius))
            action(position);
    }

    public static void ForEachPositionWithinRadiusIncludingCenter(int radius, Action<Point> action)
    {
        foreach (var position in GetAllPositionsWithinRadiusIncludingCenter(radius))
            action(position);
    }
    
    public static IEnumerable<Point> GetAllPositionsWithinRadius(int radius)
    {
        for (var x = -radius; x <= radius; x++)
        {
            for (var y = -radius; y <= radius; y++)
            {
                var position = new Point(x, y);
                if (!IsCenterPosition(position))
                    yield return position;
            }
        }
    }
    
    public static IEnumerable<Point> GetAllPositionsWithinRadiusIncludingCenter(int radius)
    {
        for (var x = -radius; x <= radius; x++)
        {
            for (var y = -radius; y <= radius; y++)
            {
                var position = new Point(x, y);
                yield return position;
            }
        }
    }
    
    public static void ValidatePositionsWithinRadius(int radius, IReadOnlySet<Point> positions)
    {
        foreach (var position in positions)
            ValidatePositionWithinRadius(radius, position);
    }

    public static void ValidatePositionWithinRadius(int radius, Point position)
    {
        if (IsCenterPosition(position)) // Todo: Point.ToString()
            throw new ArgumentException("The position (0, 0) is not a valid position.", nameof(position));

        if (position.X < -radius
            || position.X > radius
            || position.Y < -radius
            || position.Y > radius)
            throw new ArgumentException("Position coordinates must be in the range [-Radius; Radius].", nameof(position));
    }
    #endregion

    #region Neighbor count related methods
    public static int GetMaxNeighborCellCountWithinRadius(int radius) =>
        GetMaxPositionCountWithinRadius(radius);

    public static void ForEachNeighborCellCountWithinRadius(int radius, Action<int> action)
    {
        var maxNeighborCount = GetMaxNeighborCellCountWithinRadius(radius);
        for (var neighborCount = MinNeighborCount; neighborCount <= maxNeighborCount; neighborCount++)
            action(neighborCount);
    }
    
    public static void ValidateNeighborCellCountSetWithinRadius(int radius, IReadOnlySet<int> neighborCellCountSet) 
    {
        foreach (var neighborCellCount in neighborCellCountSet)
            ValidateNeighborCountWithinRadius(radius, neighborCellCount);
    }

    public static void ValidateNeighborCountWithinRadius(int radius, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));
        
        var maxNeighborCount = GetMaxNeighborCellCountWithinRadius(radius);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, maxNeighborCount, nameof(count));
    }
    #endregion
    #endregion
}