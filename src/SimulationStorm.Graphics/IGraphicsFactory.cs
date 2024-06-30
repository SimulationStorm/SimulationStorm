using SimulationStorm.Primitives;

namespace SimulationStorm.Graphics;

/// <summary>
/// Provides methods to create graphics objects.
/// </summary>
public interface IGraphicsFactory
{
    /// <summary>
    /// Creates a bitmap with the specified size.
    /// </summary>
    /// <param name="size">The desired size of the bitmap.</param>
    /// <returns>A new instance of <see cref="IBitmap"/> with the specified size.</returns>
    IBitmap CreateBitmap(Size size);
        
    /// <summary>
    /// Creates a canvas associated with the specified bitmap.
    /// </summary>
    /// <param name="bitmap">The bitmap to associate with the canvas.</param>
    /// <returns>A new instance of <see cref="ICanvas"/> associated with the specified bitmap.</returns>
    ICanvas CreateCanvas(IBitmap bitmap);

    /// <summary>
    /// Creates a paint.
    /// </summary>
    /// <returns>A new instance of <see cref="IPaint"/>.</returns>
    IPaint CreatePaint();

    /// <summary>
    /// Creates a filter that draws a drop shadow under the input content.
    /// </summary>
    /// <param name="offsetX">The X offset of the shadow.</param>
    /// <param name="offsetY">The Y offset of the shadow.</param>
    /// <param name="blurRadiusX">The blur radius for the shadow, along the X axis.</param>
    /// <param name="blurRadiusY">The blur radius for the shadow, along the Y axis.</param>
    /// <param name="color">The color of the drop shadow.</param>
    /// <returns>The drop shadow image filter.</returns>
    IImageFilter CreateDropShadowImageFilter
    (
        float offsetX,
        float offsetY,
        float blurRadiusX,
        float blurRadiusY,
        Color color
    );
}