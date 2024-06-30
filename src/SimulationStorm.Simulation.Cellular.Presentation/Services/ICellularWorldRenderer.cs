using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Models;
using SimulationStorm.Simulation.Presentation.WorldRenderer;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public interface ICellularWorldRenderer : IWorldRenderer
{
    bool IsGridLinesVisible { get; set; }

    Color GridLinesColor { get; set; }
    
    Color HoveredCellColor { get; set; }
    
    Color PressedCellColor { get; set; }
    
    (Point Cell, CellularWorldPointedCellState State)? PointedCell { get; set; }
}