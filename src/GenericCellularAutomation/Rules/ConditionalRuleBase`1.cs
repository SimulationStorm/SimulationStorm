using System;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public abstract class ConditionalRuleBase<TCellState>
(
    double applicationProbability,
    TCellState targetCellState,
    TCellState newCellState,
    TCellState neighborCellState,
    CellNeighborhood cellNeighborhood
)
    : Rule<TCellState>(applicationProbability, targetCellState, newCellState)
    where TCellState :
        IComparable,
        IComparable<TCellState>,
        IEquatable<TCellState>,
        IBinaryInteger<TCellState>,
        IMinMaxValue<TCellState>
{
    public TCellState NeighborCellState { get; } = neighborCellState;

    public CellNeighborhood CellNeighborhood { get; } = cellNeighborhood;
}