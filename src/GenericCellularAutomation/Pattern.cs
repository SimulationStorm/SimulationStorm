using System;
using System.Collections.Generic;
using System.Linq;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation;

/*
 * Pattern concept:
 * 010
 * 111
 * 010
 * - or -
 * alive|alive|alive
 * dead|dead|dead
 * alive|dead|alive
 * - the last option is more adaptive and supports all possible state words, numbers and abbreviations...
 */

public sealed class Pattern(Size size, IDictionary<Point, byte> cellStateByPositions)
{
    public Size Size { get; } = size;

    #region Public methods
    public byte GetCellState(Point cell)
    {
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.X, 0, nameof(cell.X));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.X, Width, nameof(cell.X));
        //
        // ArgumentOutOfRangeException.ThrowIfLessThan(cell.Y, 0, nameof(cell.Y));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell.Y, Height, nameof(cell.Y));

        return cellStateByPositions[cell];
    }

    public static Pattern FromString
    (
        string patternString,
        IDictionary<string, byte> cellStateByNames,
        char cellStateNameSeparator = '|',
        char lineSeparator = '\n')
    {
        var lines = ValidatePatternStringAndGetCellStateByLineNumbers(
            patternString, cellStateByNames, cellStateNameSeparator, lineSeparator);

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

    private static IReadOnlyList<IReadOnlyList<string>> ValidatePatternStringAndGetCellStateByLineNumbers
    (
        string patternString,
        IDictionary<string, byte> cellStateByNames,
        char cellStateNameSeparator,
        char lineSeparator)
    {
        var lines = patternString
            .Split(lineSeparator)
            .Select(line => line
                .Trim())
            .ToArray();
        
        var firstLineWidth = lines[0].Length;
        var areAllLinesHasTheSameLength = lines
            .Skip(1)
            .All(line => line.Length == firstLineWidth);
        
        if (!areAllLinesHasTheSameLength)
            throw new ArgumentException("All pattern lines must have the same length.", nameof(patternString));

        var cellStateNameByLineNumbers = lines
            .Select(line => line
                .Split(cellStateNameSeparator))
            .ToArray();

        var areAllCellStateNamesValid = cellStateNameByLineNumbers
            .All(cellStateNames => cellStateNames
                .All(cellStateByNames.Keys.Contains));
        
        if (!areAllCellStateNamesValid)
            throw new ArgumentException(
                $"All cell state names in the {nameof(patternString)} must be present in the {cellStateByNames}.",
                nameof(patternString));
        
        return cellStateNameByLineNumbers;
    }
}