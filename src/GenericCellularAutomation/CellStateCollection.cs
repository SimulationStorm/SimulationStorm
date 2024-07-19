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

        var newCellStateSet = CellStateSet.ToHashSet();
        newCellStateSet.Remove(cellState);
        
        return new CellStateCollection
        (
            newCellStateSet,
            DefaultCellState
        );
    }
    
    public CellStateCollection WithDefaultCellState(byte defaultCellState)
    {
        if (!CellStateSet.Contains(defaultCellState))
            throw new ArgumentException(
                $"The {defaultCellState} is not in the {nameof(CellStateCollection)}.");

        if (defaultCellState == DefaultCellState)
            throw new ArgumentException(
                $"This cell state ({defaultCellState}) is already set as default.");
        
        return new CellStateCollection
        (
            CellStateSet.ToHashSet(),
            defaultCellState
        );
    }
}