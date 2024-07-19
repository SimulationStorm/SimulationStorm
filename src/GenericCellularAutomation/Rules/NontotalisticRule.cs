using System.Collections.Generic;
using GenericCellularAutomation.Neighborhood;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Rules;

public sealed class NontotalisticRule : ConditionalRuleBase
{
    public IReadOnlySet<Point> NeighborCellPositions { get; }

    public NontotalisticRule
    (
        double applicationProbability,
        byte targetCellState,
        byte newCellState,
        byte neighborCellState,
        CellNeighborhood cellNeighborhood,
        IReadOnlySet<Point> neighborCellPositions
    )
        : base(applicationProbability, targetCellState, newCellState, neighborCellState, cellNeighborhood)
    {
        CellNeighborhood.ValidatePositionsWithinRadius(CellNeighborhood.Radius, neighborCellPositions);
        NeighborCellPositions = neighborCellPositions;
    }
}