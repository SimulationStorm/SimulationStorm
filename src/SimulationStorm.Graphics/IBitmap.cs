using System;
using SimulationStorm.Primitives;

namespace SimulationStorm.Graphics;

/// <summary>
/// Represents a raster bitmap.
/// </summary>
public interface IBitmap : IDisposable
{
    #region Properties
    /// <summary>
    /// Gets the bitmap size.
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Gets the bitmap color format.
    /// </summary>
    ColorFormat ColorFormat { get; }

    /// <summary>
    /// Gets the bitmap pixels alpha type.
    /// </summary>
    PixelAlphaType PixelAlphaType { get; }

    /// <summary>
    /// Gets the first pixel address of the bitmap.
    /// </summary>
    nint FirstPixelAddress { get; }

    /// <summary>
    /// Gets the number of bytes per one row of the bitmap.
    /// </summary>
    int BytesPerRow { get; }
    #endregion

    /// <summary>
    /// Copies the contents of the bitmap and returns the copy.
    /// </summary>
    /// <returns>The copy of the bitmap, or null on error.</returns>
    IBitmap Copy();
}