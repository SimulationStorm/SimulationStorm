using System.Collections.Generic;
using GenericCellularAutomation.Neighborhood;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleDescriptor
(
    string name,
    RuleType type,
    double applicationProbability,
    CellStateDescriptor targetCellState,
    CellStateDescriptor newCellState,
    CellStateDescriptor? neighborCellState = null,
    CellNeighborhood? cellNeighborhood = null,
    IReadOnlySet<int>? neighborCellCountSet = null,
    IReadOnlySet<Point>? neighborCellPositionSet = null)
{
    public string Name { get; } = name;

    public RuleType Type { get; } = type;

    #region Unconditional
    public double ApplicationProbability { get; } = applicationProbability;

    public CellStateDescriptor TargetCellState { get; } = targetCellState;
    
    public CellStateDescriptor NewCellState { get; } = newCellState;
    #endregion

    #region Conditional
    public CellStateDescriptor? NeighborCellState { get; } = neighborCellState;

    public CellNeighborhood? CellNeighborhood { get; } = cellNeighborhood;
    #endregion

    #region Totalistic
    public IReadOnlySet<int>? NeighborCellCountSet { get; } = neighborCellCountSet;
    #endregion

    #region Nontotalistic
    public IReadOnlySet<Point>? NeighborCellPositionSet { get; } = neighborCellPositionSet;
    #endregion
}