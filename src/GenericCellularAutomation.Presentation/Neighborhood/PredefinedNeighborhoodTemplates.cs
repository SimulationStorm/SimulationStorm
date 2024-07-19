using System;
using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.Neighborhood;

public static class PredefinedNeighborhoodTemplates
{
    public static readonly CellNeighborhoodTemplate
        Moore = new
        (
            nameof(Moore),
            (_, _, _) => true
        ),
        VonNeumann = new
        (
            nameof(VonNeumann),
            (radius, x, y) => Math.Abs(x) + Math.Abs(y) <= radius
        ),
        EuclidianDistance = new
        (
            nameof(EuclidianDistance),
            (radius, x, y) => Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5) <= radius
        ),
        Circle = new
        (
            nameof(Circle),
            (radius, x, y) => Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5) <= radius + 0.5
        ),
        Chessboard = new
        (
            nameof(Chessboard),
            (_, x, y) => (x + y) % 2 is not 0
        ),
        InvertedChessboard = new
        (
            nameof(InvertedChessboard),
            (_, x, y) => (x + y) % 2 is 0
        ),
        Grid = new
        (
            nameof(Grid),
            (_, x, y) => Math.Abs(x) is 1 || Math.Abs(y) is 1
        ),
        Cross = new
        (
            nameof(Cross),
            (_, x, y) => x is 0 || y is 0
        ),
        Saltire = new
        (
            nameof(Saltire),
            (_, x, y) => Math.Abs(x) == Math.Abs(y)
        ),
        Star = new
        (
            nameof(Star),
            (_, x, y) => x is 0 || y is 0 || Math.Abs(x) == Math.Abs(y)
        );
    
    public static readonly IEnumerable<CellNeighborhoodTemplate> All = new[]
    {
        Moore,
        VonNeumann,
        EuclidianDistance,
        Circle,
        Chessboard,
        InvertedChessboard,
        Grid,
        Cross,
        Saltire,
        Star
    };
}