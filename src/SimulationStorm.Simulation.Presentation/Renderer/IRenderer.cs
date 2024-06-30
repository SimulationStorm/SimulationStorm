using System;
using SimulationStorm.Graphics;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public interface IRenderer
{
    IBitmap? RenderedImage { get; }

    event EventHandler? RenderingStarted;
    
    event EventHandler<RenderingCompletedEventArgs>? RenderingCompleted;

    void RequestRerender();
}