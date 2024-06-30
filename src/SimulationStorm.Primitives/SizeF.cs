using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents an immutable size with floating-point width and height.
/// </summary>
public readonly struct SizeF : IEquatable<SizeF>, IComparable<SizeF>
{
    /// <summary>
    /// Represents a size with width and height set to 0.
    /// </summary>
    public static readonly SizeF Zero = new(0, 0);

    #region Properties
    /// <summary>
    /// Gets the width of the size.
    /// </summary>
    public float Width { get; }

    /// <summary>
    /// Gets the height of the size.
    /// </summary>
    public float Height { get; }

    /// <summary>
    /// Gets the area of the size.
    /// </summary>
    public float Area => Width * Height;
    #endregion
    
    /// <summary>
    /// Creates the new size with the given width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    [JsonConstructor]
    public SizeF(float width, float height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0, nameof(width));
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 0, nameof(height));

        Width = width;
        Height = height;
    }
    
    #region Operators
    /// <summary>
    /// Multiplies the width and height of a size by the specified float number.
    /// </summary>
    /// <param name="size">The size to scale.</param>
    /// <param name="number">The number to scale the size by.</param>
    public static SizeF operator *(SizeF size, float number) => new(size.Width * number, size.Height * number);
    
    /// <summary>
    /// Divides the width and height of a size by the specified float number.
    /// </summary>
    /// <param name="size">The size to divide.</param>
    /// <param name="number">The number to divide the size by.</param>
    public static SizeF operator /(SizeF size, float number) => new(size.Width / number, size.Height / number);
    #endregion

    /// <summary>
    /// Converts a SizeF structure to a Size structure by rounding its components to the nearest integer values.
    /// </summary>
    /// <param name="sizeF">The size to convert.</param>
    public static Size ToSize(SizeF sizeF) => new((int)Math.Round(sizeF.Width), (int)Math.Round(sizeF.Height));

    #region Equality Methods
    /// <inheritdoc />
    public bool Equals(SizeF other) => Width.Equals(other.Width) && Height.Equals(other.Height);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is SizeF other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    /// Determines whether two sizes are equal.
    /// </summary>
    public static bool operator ==(SizeF left, SizeF right) => left.Equals(right);

    /// <summary>
    /// Determines whether two sizes are not equal.
    /// </summary>
    public static bool operator !=(SizeF left, SizeF right) => !left.Equals(right);
    #endregion
    
    #region Comparison methods
    public int CompareTo(SizeF other)
    {
        var widthComparison = Width.CompareTo(other.Width);
        return widthComparison is not 0 ? widthComparison : Height.CompareTo(other.Height);
    }

    public int CompareTo(object? obj) => ReferenceEquals(null, obj) ? 1 :
        obj is SizeF other ? CompareTo(other) :
            throw new ArgumentException($"Object must be of type {nameof(SizeF)}");

    public static bool operator <(SizeF left, SizeF right) => left.CompareTo(right) < 0;

    public static bool operator >(SizeF left, SizeF right) => left.CompareTo(right) > 0;

    public static bool operator <=(SizeF left, SizeF right) => left.CompareTo(right) <= 0;

    public static bool operator >=(SizeF left, SizeF right) => left.CompareTo(right) >= 0;
    #endregion
    
    /// <summary>
    /// Deconstructs the size into its width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public void Deconstruct(out float width, out float height)
    {
        width = Width;
        height = Height;
    }
}