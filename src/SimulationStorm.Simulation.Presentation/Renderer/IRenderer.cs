using System;
using SimulationStorm.Graphics;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public interface IRenderer
{
    IBitmap? RenderedImage { get; }

    #region Events
    event EventHandler? RenderingStarting;
    
    event EventHandler<RenderingCompletedEventArgs>? RenderingCompleted;
    #endregion

    void RequestRerender();
}