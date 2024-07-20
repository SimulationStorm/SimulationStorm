using System;
using System.Collections.Generic;
using System.Linq;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation;

public sealed class Pattern
{
    private readonly IReadOnlyDictionary<Point, byte> _cellStateByPositions;

    public Pattern(Size size, IReadOnlyDictionary<Point, byte> cellStateByPositions)
    {
        Size = size;

        ValidateCellStateByPositions(size, cellStateByPositions);
        _cellStateByPositions = cellStateByPositions;
    }

    public Size Size { get; }

    #region Public methods
    public byte GetCellState(Point cellPosition)
    {
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.X, 0, nameof(cell.X));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.X, Width, nameof(cell.X));
        //
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.Y, 0, nameof(cell.Y));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.Y, Height, nameof(cell.Y));

        return _cellStateByPositions[cellPosition];
    }

    public static Pattern FromScheme
    (
        string scheme,
        IDictionary<string, byte> cellStateByNames,
        char cellStateNameSeparator = '|',
        char lineSeparator = '\n')
    {
        var lines = ValidatePatternSchemeAndGetCellStateByLineNumbers(
            scheme, cellStateByNames, cellStateNameSeparator, lineSeparator);

        var patternSize = new Size(lines[0].Count, lines.Count);

        var cellStateByPositions = new Dictionary<Point, byte>();
        
        for (var x = 0; x < patternSize.Width; x++)
        {
            for (var y = 0; y < patternSize.Height; y++)
            {
                var cellPosition = new Point(x, y);
                
                var cellStateName = lines[y][x];
                var cellState = cellStateByNames[cellStateName];

                cellStateByPositions[cellPosition] = cellState;
            }
        }

        return new Pattern(patternSize, cellStateByPositions);
    }
    #endregion

    private static IReadOnlyList<IReadOnlyList<string>> ValidatePatternSchemeAndGetCellStateByLineNumbers
    (
        string scheme,
        IDictionary<string, byte> cellStateByNames,
        char cellStateNameSeparator,
        char lineSeparator)
    {
        var lines = scheme
            .Split(lineSeparator)
            .Select(line => line
                .Trim())
            .ToArray();
        
        var firstLineWidth = lines[0].Length;
        var areAllLinesHasTheSameLength = lines
            .Skip(1)
            .All(line => line.Length == firstLineWidth);
        
        if (!areAllLinesHasTheSameLength)
            throw new ArgumentException("All pattern lines must have the same length.", nameof(scheme));

        var cellStateNameByLineNumbers = lines
            .Select(line => line
                .Split(cellStateNameSeparator))
            .ToArray();

        var areAllCellStateNamesValid = cellStateNameByLineNumbers
            .All(cellStateNames => cellStateNames
                .All(cellStateByNames.Keys.Contains));
        
        if (!areAllCellStateNamesValid)
            throw new ArgumentException(
                $"All cell state names in the {nameof(scheme)} must be present in the {cellStateByNames}.",
                nameof(scheme));
        
        return cellStateNameByLineNumbers;
    }

    private static void ValidateCellStateByPositions
    (
        Size patternSize,
        IReadOnlyDictionary<Point, byte> cellStateByPositions)
    {
        var patternRect = new Rect(0, 0, patternSize.Width, patternSize.Height);
        if (!cellStateByPositions.Keys.All(patternRect.Contains))
            throw new ArgumentException(
                "All cell positions must be within a pattern size.",
                nameof(cellStateByPositions));
    }
}