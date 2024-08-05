using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Neighborhood;

public sealed partial class CellNeighborhoodViewModel(NeighborhoodOptions options) : ObservableObject
{
    #region Properties
    [ObservableProperty] private int _radius;
    
    [ObservableProperty] private CellNeighborhoodTemplate? _template;

    public IReadOnlySet<Point> SelectedPositions => (IReadOnlySet<Point>)_selectedPositions;

    public Range<int> RadiusRange => options.RadiusRange;
    #endregion

    #region Fields
    private readonly ISet<Point> _selectedPositions = new HashSet<Point>();
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSelectPosition))]
    private void SelectPosition(Point position)
    {
        _selectedPositions.Add(position);
        OnPropertyChanged(nameof(SelectedPositions));
    }
    private bool CanSelectPosition(Point position) =>
        !_selectedPositions.Contains(position);

    [RelayCommand(CanExecute = nameof(CanUnselectPosition))]
    private void UnselectPosition(Point position)
    {
        _selectedPositions.Remove(position);
        OnPropertyChanged(nameof(SelectedPositions));
    }
    private bool CanUnselectPosition(Point position) =>
        _selectedPositions.Contains(position);
    
    [RelayCommand(CanExecute = nameof(CanSelectAllPositions))]
    private void SelectAllPositions()
    {
        CellNeighborhood.ForEachPositionWithinRadius(Radius, position => _selectedPositions.Add(position));
        OnPropertyChanged(nameof(SelectedPositions));
    }
    private bool CanSelectAllPositions() =>
        _selectedPositions.Count < CellNeighborhood.GetMaxPositionCountWithinRadius(Radius);
    
    [RelayCommand(CanExecute = nameof(CanReset))]
    private void Reset()
    {
        _selectedPositions.Clear();
        OnPropertyChanged(nameof(SelectedPositions));
    }
    private bool CanReset() =>
        _selectedPositions.Count is not 0;

    [RelayCommand]
    private void ApplyTemplate(CellNeighborhoodTemplate template)
    {
        var newNeighborhood = template.BuildNeighborhood(Radius);
        
        _selectedPositions.Clear();
        _selectedPositions.AddAll(newNeighborhood.UsedPositions);
        OnPropertyChanged(nameof(SelectedPositions));
    }
    #endregion
}