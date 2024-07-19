using GenericCellularAutomation.Neighborhood;

namespace GenericCellularAutomation.Rules;

public abstract class ConditionalRuleBase
(
    double applicationProbability,
    byte targetCellState,
    byte newCellState,
    byte neighborCellState,
    CellNeighborhood cellNeighborhood
)
    : Rule(applicationProbability, targetCellState, newCellState)
{
    public byte NeighborCellState { get; } = neighborCellState;

    public CellNeighborhood CellNeighborhood { get; } = cellNeighborhood;
}