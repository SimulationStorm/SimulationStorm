using System;
using System.Collections.Generic;
using GenericCellularAutomation.Neighborhood;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Rules;

public sealed class Rule
{
    #region Properties
    public RuleType Type { get; }

    #region Unconditional
    public double ApplicationProbability { get; }

    public byte TargetCellState { get; }

    public byte NewCellState { get; }
    #endregion

    #region Conditional
    public byte? NeighborCellState { get; }

    public CellNeighborhood? CellNeighborhood { get; }
    #endregion
    
    #region Totalistic
    public IReadOnlySet<int>? NeighborCellCountSet { get; }
    #endregion

    #region Nontotalistic
    public IReadOnlySet<Point>? NeighborCellPositionSet { get; }
    #endregion
    #endregion
    
    public Rule
    (
        RuleType type,
        double applicationProbability,
        byte targetCellState,
        byte newCellState,
        byte? neighborCellState = null,
        CellNeighborhood? cellNeighborhood = null,
        IReadOnlySet<int>? neighborCellCountSet = null,
        IReadOnlySet<Point>? neighborCellPositionSet = null)
    {
        Type = type;
        
        ArgumentOutOfRangeException.ThrowIfLessThan(applicationProbability, 0, nameof(applicationProbability));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(applicationProbability, 1, nameof(applicationProbability));
        ApplicationProbability = applicationProbability;
        
        TargetCellState = targetCellState;
        NewCellState = newCellState;

        if (type is RuleType.Totalistic or RuleType.Nontotalistic)
        {
            if (!neighborCellState.HasValue)
                throw new ArgumentNullException(nameof(neighborCellState));
            
            NeighborCellState = neighborCellState;
            
            ArgumentNullException.ThrowIfNull(cellNeighborhood);
            CellNeighborhood = cellNeighborhood;
        }

        switch (type)
        {
            case RuleType.Totalistic:
            {
                ArgumentNullException.ThrowIfNull(neighborCellCountSet);
                CellNeighborhood.ValidateNeighborCellCountSetWithinRadius(
                    CellNeighborhood!.Radius, neighborCellCountSet);
                
                NeighborCellCountSet = neighborCellCountSet;
                break;
            }
            case RuleType.Nontotalistic:
            {
                ArgumentNullException.ThrowIfNull(neighborCellPositionSet);
                CellNeighborhood.ValidatePositionsWithinRadius(
                    CellNeighborhood!.Radius, neighborCellPositionSet);
                
                NeighborCellPositionSet = neighborCellPositionSet;
                break;
            }
        }
    }
}