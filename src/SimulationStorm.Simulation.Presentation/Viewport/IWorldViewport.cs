using System;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.Viewport;

/// <summary>
/// Provides the size of the viewport and notifies when its size changes.
/// </summary>
public interface IWorldViewport
{
    /// <summary>
    /// Gets the viewport size.
    /// </summary>
    Size Size { get; set; }
    
    /// <summary>
    /// Occurs when the viewport size changes.
    /// </summary>
    event EventHandler<ViewportSizeChangedEventArgs>? SizeChanged;
}