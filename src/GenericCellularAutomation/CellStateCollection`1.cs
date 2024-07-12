using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation;

public class CellStateCollection<TCellState> where TCellState : IBinaryInteger<TCellState>
{
    public IReadOnlySet<TCellState> CellStateSet { get; set; }
    
    public TCellState DefaultCellState { get; set; }

    public CellStateCollection(IReadOnlySet<TCellState> cellStateSet, TCellState defaultCellState)
    {
        if (!cellStateSet.Contains(defaultCellState))
            throw new ArgumentException(
                $"The {nameof(defaultCellState)} must be in the ${nameof(cellStateSet)}.",
                nameof(defaultCellState));

        CellStateSet = cellStateSet;
        DefaultCellState = defaultCellState;
    }
}