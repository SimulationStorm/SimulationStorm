using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererOptions : ISimulationRendererOptions
{
    public bool IsRenderingEnabled { get; init; }

    public Range<int> RenderingIntervalRange { get; init; }
    
    public int RenderingInterval { get; init; }
    
    public Color DeadCellColor { get; init; }
    
    public Color AliveCellColor { get; init; }
}