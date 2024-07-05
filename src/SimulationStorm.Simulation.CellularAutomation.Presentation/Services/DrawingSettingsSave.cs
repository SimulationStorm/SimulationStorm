using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public class DrawingSettingsSave<TCellState>
{
    public bool IsDrawingEnabled { get; init; }

    public DrawingShape DrawingBrushShape { get; init; }

    public int DrawingBrushRadius { get; init; }

    public TCellState DrawingBrushCellState { get; init; } = default!;
}