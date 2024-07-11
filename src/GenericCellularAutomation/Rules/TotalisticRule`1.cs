using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public sealed class TotalisticRule<TCellState> : ConditionalRuleBase<TCellState> where TCellState :
    IComparable,
    IComparable<TCellState>,
    IEquatable<TCellState>,
    IBinaryInteger<TCellState>,
    IMinMaxValue<TCellState>
{
    public IReadOnlySet<int> NeighborCellCountSet { get; }

    public TotalisticRule
    (
        double applicationProbability,
        TCellState targetCellState,
        TCellState newCellState,
        TCellState neighborCellState,
        CellNeighborhood cellNeighborhood,
        IReadOnlySet<int> neighborCellCountSet
    )
        : base(applicationProbability, targetCellState, newCellState, neighborCellState, cellNeighborhood)
    {
        CellNeighborhood.ValidateNeighborCellCountSetWithinRadius(CellNeighborhood.Radius, neighborCellCountSet);
        NeighborCellCountSet = neighborCellCountSet;
    }
}