using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Neighborhood;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules.Models;

public sealed partial class RuleModel : NamedIndexedObservableObject
{
    [ObservableProperty] private RuleType _type;

    #region Unconditional
    [ObservableProperty] private double _applicationProbability;

    [ObservableProperty] private byte _targetCellState;
    
    [ObservableProperty] private byte _newCellState;
    #endregion

    #region Conditional
    [ObservableProperty] private byte? _neighborCellState;

    [ObservableProperty] private CellNeighborhood? _cellNeighborhood;
    #endregion

    #region Totalistic
    public ObservableCollection<int> NeighborCellCountCollection { get; } = [];
    #endregion
    
    #region Nontotalistic
    public ObservableCollection<Point> NeighborCellPositionCollection { get; } = [];
    #endregion
}