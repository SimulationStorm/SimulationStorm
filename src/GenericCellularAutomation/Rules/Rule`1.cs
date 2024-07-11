using System;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public class Rule<TCellState>
(
    double applicationProbability,
    TCellState targetCellState,
    TCellState newCellState
)
    where TCellState :
        IComparable,
        IComparable<TCellState>,
        IEquatable<TCellState>,
        IBinaryInteger<TCellState>,
        IMinMaxValue<TCellState>
{
    public double ApplicationProbability { get; } = applicationProbability;

    public TCellState TargetCellState { get; } = targetCellState;

    public TCellState NewCellState { get; } = newCellState;
}