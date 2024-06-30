using System;

namespace SimulationStorm.Graphics;

/// <summary>
/// Holds the information about how to draw geometries.
/// </summary>
public interface IPaint : IDisposable
{
    /// <summary>
    /// Gets or sets the paint's color.
    /// </summary>
    Color Color { get; set; }

    /// <summary>
    /// Gets or sets the paint's style.
    /// </summary>
    PaintStyle Style { get; set; }
    
    /// <summary>
    /// Gets or sets the paint's stroke width.
    /// </summary>
    float StrokeWidth { get; set; }
    
    /// <summary>
    /// Gets or sets the paint's image filter.
    /// </summary>
    IImageFilter ImageFilter { get; set; }
}