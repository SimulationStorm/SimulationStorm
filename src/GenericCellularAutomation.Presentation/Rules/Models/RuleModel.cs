using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Neighborhood;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Rules.Descriptors;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules.Models;

public sealed partial class RuleModel : NamedIndexedObservableObject
{
    [ObservableProperty] private RuleType _type;

    #region Unconditional
    [ObservableProperty] private double _applicationProbability;

    [ObservableProperty] private CellStateModel _targetCellState = null!;
    
    [ObservableProperty] private CellStateModel _newCellState = null!;
    #endregion

    #region Conditional
    [ObservableProperty] private CellStateModel? _neighborCellState;

    [ObservableProperty] private CellNeighborhood? _cellNeighborhood;
    #endregion

    #region Totalistic
    public ObservableCollection<int> NeighborCellCountCollection { get; } = [];
    #endregion
    
    #region Nontotalistic
    public ObservableCollection<Point> NeighborCellPositionCollection { get; } = [];
    #endregion

    public Rule ToRule() => new
    (
        Type,
        ApplicationProbability,
        TargetCellState.CellState,
        NewCellState.CellState,
        NeighborCellState?.CellState,
        CellNeighborhood,
        Type is RuleType.Totalistic ? NeighborCellCountCollection.ToHashSet() : null,
        Type is RuleType.Nontotalistic ? NeighborCellPositionCollection.ToHashSet() : null
    );
}