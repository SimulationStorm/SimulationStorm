using CommunityToolkit.Mvvm.ComponentModel;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Patterns;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public partial class GameOfLifeDrawingSettings(GameOfLifeDrawingOptions options) :
    DrawingSettings<GameOfLifeCellState>(options)
{
    #region Properties
    [ObservableProperty] private NamedPattern? _currentPattern = options.Pattern;

    [ObservableProperty] private bool _placePatternWithOverlay = options.PlacePatternWithOverlay;
    #endregion

    protected override void OnIsDrawingEnabledChanged()
    {
        if (IsDrawingEnabled)
            CurrentPattern = null;
    }

    partial void OnCurrentPatternChanged(NamedPattern? value)
    {
        if (CurrentPattern is not null)
            IsDrawingEnabled = false;
    }
}