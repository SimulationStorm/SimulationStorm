namespace SimulationStorm.Graphics;

/// <summary>
/// Describes how to interpret the alpha component of a pixel.
/// </summary>
public enum PixelAlphaType
{
    /// <summary>
    /// All pixels are stored as opaque.
    /// </summary>
    Opaque,
    /// <summary>
    /// All pixels have their alpha premultiplied in their color components.
    /// This is the natural format for the rendering target pixels.
    /// </summary>
    Premultiplied,
    /// <summary>
    ///     <para>All pixels have their color components stored without any regard to the alpha. e.g. this is the default configuration for PNG images.</para>
    ///     <para>This alpha-type is ONLY supported for input images. Rendering cannot generate this on output.</para>
    /// </summary>
    Unpremultiplied
}