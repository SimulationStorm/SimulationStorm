using SimulationStorm.Graphics;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererSave
{
    public bool IsRenderingEnabled { get; init; }
    
    public int RenderingInterval { get; init; }

    // public (Color DeadCellColor, Color AliveCellColor) CellColors { get; init; }

    public Color DeadCellColor { get; init; }

    public Color AliveCellColor { get; init; }
}