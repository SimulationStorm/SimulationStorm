using System;

namespace SimulationStorm.Graphics;

/// <summary>
/// Provides useful methods to work with colors.
/// </summary>
public static class ColorUtils
{
    /// <summary>
    /// Generates a random color.
    /// </summary>
    /// <param name="randomizeAlpha">Whether to randomize color alpha component. The default value is false.</param>
    /// <returns>A random color.</returns>
    public static Color GenerateRandomColor(bool randomizeAlpha = false) => new
    (
        (byte)Random.Shared.Next(0, 256),
        (byte)Random.Shared.Next(0, 256),
        (byte)Random.Shared.Next(0, 256),
        randomizeAlpha ? (byte)Random.Shared.Next(0, 256) : (byte)255
    );
}