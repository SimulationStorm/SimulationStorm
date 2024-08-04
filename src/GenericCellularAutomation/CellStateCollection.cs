using System;
using System.Collections.Generic;

namespace GenericCellularAutomation;

public sealed class CellStateCollection
{
    #region Properties
    public IReadOnlySet<GcaCellState> CellStateSet { get; }
    
    public GcaCellState DefaultCellState { get; }
    #endregion

    public CellStateCollection(IReadOnlySet<GcaCellState> cellStateSet, GcaCellState defaultCellState)
    {
        if (!cellStateSet.Contains(defaultCellState))
            throw new ArgumentException(
                $"The {nameof(defaultCellState)} must be in the ${nameof(cellStateSet)}.",
                nameof(defaultCellState));

        CellStateSet = cellStateSet;
        DefaultCellState = defaultCellState;
    }
}