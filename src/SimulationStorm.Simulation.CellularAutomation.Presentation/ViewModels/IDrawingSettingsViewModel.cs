using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

public interface IDrawingSettingsViewModel
{
    #region Properties
    bool IsDrawingModeEnabled { get; set; }
    
    int BrushRadius { get; set; }
    
    Range<int> BrushRadiusRange { get; }

    IEnumerable<DrawingShape> BrushShapes { get; }
    
    IEnumerable<object> BrushCellStates { get; }
    #endregion

    #region Commands
    IRelayCommand<DrawingShape> ChangeBrushShapeCommand { get; }
    
    IRelayCommand<object> ChangeBrushCellStateCommand { get; }
    #endregion
}