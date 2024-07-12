using System.Numerics;
using GenericCellularAutomation.Neighborhood;

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
    where TCellState : IBinaryInteger<TCellState>
{
    public TCellState NeighborCellState { get; } = neighborCellState;

    public CellNeighborhood CellNeighborhood { get; } = cellNeighborhood;
}