using System;
using System.Text.Json.Serialization;
using SimulationStorm.Graphics.Extensions;

namespace SimulationStorm.Graphics;

/// <summary>
/// Represents a 32-bit RGBA color.
/// </summary>
public readonly struct Color : IEquatable<Color>
{
    public static readonly Color Empty = new(0, 0, 0, 0);

    #region Properties
    /// <summary>
    /// Gets the color red component.
    /// </summary>
    public byte Red { get; }

    /// <summary>
    /// Gets the color green component.
    /// </summary>
    public byte Green { get; }

    /// <summary>
    /// Gets the color blue component.
    /// </summary>
    public byte Blue { get; }

    /// <summary>
    /// Gets the color alpha component.
    /// </summary>
    public byte Alpha { get; }
    #endregion
    
    /// <param name="red">The color red component.</param>
    /// <param name="green">The color green component.</param>
    /// <param name="blue">The color blue component.</param>
    /// <param name="alpha">The color alpha component.</param>
    [JsonConstructor]
    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    #region Methods
    /// <summary>
    /// Returns the inverted color: (255 - Red, 255 - Green, 255 - Blue, Alpha).
    /// </summary>
    /// <returns>The inverted SKColor.</returns>
    public Color Inverted() => new
    (
        (byte)(255 - Red),
        (byte)(255 - Green),
        (byte)(255 - Blue),
        Alpha
    );

    /// <summary>
    /// Returns the result of the linear interpolation between this color and to color by amount weight.
    /// </summary>
    /// <param name="to">The destination Color for interpolation.</param>
    /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
    /// <returns>The resulting color of the interpolation.</returns>
    public Color LinearlyInterpolated(Color to, float weight) => new
    (
        (byte)MathExtensions.LinearlyInterpolate(Red, to.Red, weight),
        (byte)MathExtensions.LinearlyInterpolate(Green, to.Green, weight),
        (byte)MathExtensions.LinearlyInterpolate(Blue, to.Blue, weight),
        (byte)MathExtensions.LinearlyInterpolate(Alpha, to.Alpha, weight)
    );

    #region Equality methods
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Color other && Equals(other);
    
    /// <inheritdoc/>
    public bool Equals(Color other) =>
        Red == other.Red && Green == other.Green && Blue == other.Blue && Alpha == other.Alpha;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Red, Green, Blue, Alpha);

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !(left == right);
    #endregion
    #endregion
}