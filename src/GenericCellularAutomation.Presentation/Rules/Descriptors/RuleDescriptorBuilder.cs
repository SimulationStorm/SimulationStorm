using System.Collections.Generic;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Neighborhood;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleDescriptorBuilder : IFluentBuilder<RuleDescriptor>
{
    #region Fields
    private string _name = string.Empty;
    
    private RuleType _type;

    #region Unconditional
    private double _applicationProbability;

    private CellStateDescriptor _targetCellState = null!;

    private CellStateDescriptor _newCellState = null!;
    #endregion

    #region Conditional
    private CellStateDescriptor? _neighborCellState;

    private CellNeighborhood? _cellNeighborhood;
    #endregion

    #region Totalistic
    private readonly ISet<int> _neighborCellCountSet = new HashSet<int>();
    #endregion

    #region Nontotalistic
    private readonly ISet<Point> _neighborCellPositionSet = new HashSet<Point>();
    #endregion
    #endregion

    #region Methods
    public RuleDescriptorBuilder HasName(string name)
    {
        _name = name;
        return this;
    }
    
    public RuleDescriptorBuilder HasType(RuleType type)
    {
        _type = type;
        return this;
    }

    public RuleDescriptorBuilder HasApplicationProbability(double applicationProbability)
    {
        _applicationProbability = applicationProbability;
        return this;
    }

    public RuleDescriptorBuilder HasTargetCellState(CellStateDescriptor targetCellState)
    {
        _targetCellState = targetCellState;
        return this;
    }

    public RuleDescriptorBuilder HasNewCellState(CellStateDescriptor newCellState)
    {
        _newCellState = newCellState;
        return this;
    }

    public RuleDescriptorBuilder HasNeighborCellState(CellStateDescriptor neighborCellState)
    {
        _neighborCellState = neighborCellState;
        return this;
    }

    public RuleDescriptorBuilder HasCellNeighborhood(CellNeighborhood cellNeighborhood)
    {
        _cellNeighborhood = cellNeighborhood;
        return this;
    }

    public RuleDescriptorBuilder HasNeighborCellCount(params int[] neighborCellCountCollection)
    {
        _neighborCellCountSet.AddAll(neighborCellCountCollection);
        return this;
    }

    public RuleDescriptorBuilder HasNeighborCellPosition(params Point[] neighborCellPositionCollection)
    {
        _neighborCellPositionSet.AddAll(neighborCellPositionCollection);
        return this;
    }

    public RuleDescriptor Build() => new
    (
        _name,
        _type,
        _applicationProbability,
        _targetCellState,
        _newCellState,
        _neighborCellState,
        _cellNeighborhood,
        (IReadOnlySet<int>)_neighborCellCountSet,
        (IReadOnlySet<Point>)_neighborCellPositionSet
    );
    #endregion
}