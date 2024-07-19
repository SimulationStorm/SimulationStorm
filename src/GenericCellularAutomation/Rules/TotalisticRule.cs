using System.Collections.Generic;
using GenericCellularAutomation.Neighborhood;

namespace GenericCellularAutomation.Rules;

public sealed class TotalisticRule : ConditionalRuleBase
{
    public IReadOnlySet<int> NeighborCellCountSet { get; }

    public TotalisticRule
    (
        double applicationProbability,
        byte targetCellState,
        byte newCellState,
        byte neighborCellState,
        CellNeighborhood cellNeighborhood,
        IReadOnlySet<int> neighborCellCountSet
    )
        : base(applicationProbability, targetCellState, newCellState, neighborCellState, cellNeighborhood)
    {
        CellNeighborhood.ValidateNeighborCellCountSetWithinRadius(CellNeighborhood.Radius, neighborCellCountSet);
        NeighborCellCountSet = neighborCellCountSet;
    }
}