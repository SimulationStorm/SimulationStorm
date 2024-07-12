using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Neighborhood;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Rules;

public sealed class NontotalisticRule<TCellState> : ConditionalRuleBase<TCellState>
    where TCellState : IBinaryInteger<TCellState>
{
    public IReadOnlySet<Point> NeighborCellPositions { get; }

    public NontotalisticRule
    (
        double applicationProbability,
        TCellState targetCellState,
        TCellState newCellState,
        TCellState neighborCellState,
        CellNeighborhood cellNeighborhood,
        IReadOnlySet<Point> neighborCellPositions
    )
        : base(applicationProbability, targetCellState, newCellState, neighborCellState, cellNeighborhood)
    {
        CellNeighborhood.ValidatePositionsWithinRadius(CellNeighborhood.Radius, neighborCellPositions);
        NeighborCellPositions = neighborCellPositions;
    }
}