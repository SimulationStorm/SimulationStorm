using CommunityToolkit.Mvvm.ComponentModel;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public partial class DrawingSettings<TCellState>(IDrawingOptions<TCellState> options) :
    ObservableObject, IDrawingSettings<TCellState>
{
    #region Properties
    [ObservableProperty] private bool _isDrawingEnabled = options.IsDrawingEnabled;
    
    [ObservableProperty] private DrawingShape _brushShape = options.BrushShape;

    [ObservableProperty] private int _brushRadius = options.BrushRadius;

    [ObservableProperty] private TCellState _brushCellState = options.BrushCellState;
    #endregion

    partial void OnIsDrawingEnabledChanged(bool value) => OnIsDrawingEnabledChanged();
    protected virtual void OnIsDrawingEnabledChanged() { }
}