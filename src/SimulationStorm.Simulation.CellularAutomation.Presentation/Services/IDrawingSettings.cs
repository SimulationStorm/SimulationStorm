using System.ComponentModel;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public interface IDrawingSettings<TCellState> : INotifyPropertyChanged
{
    bool IsDrawingEnabled { get; set; }

    DrawingShape BrushShape { get; set; }

    int BrushRadius { get; set; }

    TCellState BrushCellState { get; set; }
}