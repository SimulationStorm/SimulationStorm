using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation;

public sealed class CellStateCollection
{
    public IReadOnlySet<byte> CellStateSet { get; }
    
    public byte DefaultCellState { get; }

    public CellStateCollection(IReadOnlySet<byte> cellStateSet, byte defaultCellState)
    {
        if (!cellStateSet.Contains(defaultCellState))
            throw new ArgumentException(
                $"The {nameof(defaultCellState)} must be in the ${nameof(cellStateSet)}.",
                nameof(defaultCellState));

        CellStateSet = cellStateSet;
        DefaultCellState = defaultCellState;
    }

    public CellStateCollection WithCellState(byte cellState)
    {
        if (CellStateSet.Contains(cellState))
            throw new ArgumentException(
                $"The {cellState} is already in the {nameof(CellStateCollection)}.");

        return new CellStateCollection
        (
            new HashSet<byte>(CellStateSet) { cellState },
            DefaultCellState
        );
    }
    
    public CellStateCollection WithoutCellState(byte cellState)
    {
        if (!CellStateSet.Contains(cellState))
            throw new ArgumentException(
                $"The {cellState} is not in the {nameof(CellStateCollection)}.");

        var newCellStateSet = new HashSet<byte>(CellStateSet);
        newCellStateSet.Remove(cellState);
        
        return new CellStateCollection
        (
            newCellStateSet,
            DefaultCellState
        );
    }
}