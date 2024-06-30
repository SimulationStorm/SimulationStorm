using SimulationStorm.Graphics;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererState
{
    public bool IsRenderingEnabled { get; init; }
    
    public int RenderingInterval { get; init; }

    public (Color DeadCellColor, Color AliveCellColor) CellColors { get; init; }
}