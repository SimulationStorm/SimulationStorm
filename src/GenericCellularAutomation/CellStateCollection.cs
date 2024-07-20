using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation;

public sealed class CellStateCollection
{
    #region Properties
    public IReadOnlySet<byte> CellStateSet { get; }
    
    public byte DefaultCellState { get; }
    #endregion

    public CellStateCollection(IReadOnlySet<byte> cellStateSet, byte defaultCellState)
    {
        if (!cellStateSet.Contains(defaultCellState))
            throw new ArgumentException(
                $"The {nameof(defaultCellState)} must be in the ${nameof(cellStateSet)}.",
                nameof(defaultCellState));

        CellStateSet = cellStateSet;
        DefaultCellState = defaultCellState;
    }
}