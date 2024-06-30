namespace SimulationStorm.Graphics.Extensions;

/// <summary>
/// Provides some math extensions.
/// </summary>
public static class MathExtensions
{
    /// <summary>
    /// Linearly interpolates between two values by a normalized value.
    /// </summary>
    /// <param name="from">The start value for interpolation.</param>
    /// <param name="to">The destination value for interpolation.</param>
    /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
    /// <returns>The resulting value of the interpolation.</returns>
    public static float LinearlyInterpolate(float from, float to, float weight) => from + (to - from) * weight;
}