using System;
using System.Collections.Generic;
using System.Linq;
using SimulationStorm.Primitives;

namespace SimulationStorm.GameOfLife.DataTypes;

public class GameOfLifePattern(Size size, IReadOnlySet<Point> aliveCells)
{
    public Size Size { get; } = size;

    #region Public methods
    public GameOfLifeCellState GetCellState(Point cell)
    {
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.X, 0, nameof(cell.X));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.X, Width, nameof(cell.X));
        //
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.Y, 0, nameof(cell.Y));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.Y, Height, nameof(cell.Y));

        return aliveCells.Contains(cell) ? GameOfLifeCellState.Alive : GameOfLifeCellState.Dead;
    }

    public static GameOfLifePattern FromString(string patternString, char deadCellChar, char aliveCellChar)
    {
        var lines = ValidatePatternStringAndGetLines(patternString, deadCellChar, aliveCellChar);

        int width = lines[0].Length,
            height = lines.Count;
        
        var aliveCells = new HashSet<Point>();
        
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                if (lines[y][x] == aliveCellChar)
                    aliveCells.Add(new Point(x, y));

        return new GameOfLifePattern(new Size(width, height), aliveCells);
    }
    #endregion

    private static IReadOnlyList<string> ValidatePatternStringAndGetLines(
        string patternString, char deadCellChar, char aliveCellChar)
    {
        var lines = patternString.Split('\n').Select(row => row.Trim()).ToArray();
        
        var firstLineWidth = lines[0].Length;
        if (lines.Skip(1).Any(line => line.Length != firstLineWidth))
            throw new ArgumentException("All lines must have the same length.", nameof(patternString));
        
        if (lines.Any(line => line.Any(c => c != deadCellChar && c != aliveCellChar)))
            throw new ArgumentException("Must contain dead and alive cell characters only.", nameof(patternString));
        
        return lines;
    }
}