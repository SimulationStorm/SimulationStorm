using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents an immutable size with integer width and height.
/// </summary>
public readonly struct Size : IEquatable<Size>, IComparable<Size>, IComparable
{
    /// <summary>
    /// Represents a size with width and height set to 0.
    /// </summary>
    public static readonly Size Zero = new(0, 0);

    #region Properties
    /// <summary>
    /// Gets the width of the size.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the size.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the area of the size.
    /// </summary>
    public int Area => Width * Height;
    #endregion

    /// <summary>
    /// Creates the new size with the given width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    [JsonConstructor]
    public Size(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0, nameof(width));
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 0, nameof(height));
        
        Width = width;
        Height = height;
    }

    #region Operators
    /// <summary>
    /// Multiplies the width and height of a size by the specified integer number.
    /// </summary>
    /// <param name="size">The size to scale.</param>
    /// <param name="number">The number to scale the size by.</param>
    public static Size operator *(Size size, int number) => new(size.Width * number, size.Height * number);
    
    /// <summary>
    /// Divides the width and height of a size by the specified integer number.
    /// </summary>
    /// <param name="size">The size to divide.</param>
    /// <param name="number">The number to divide the size by.</param>
    public static Size operator /(Size size, int number) => new(size.Width / number, size.Height / number);
    #endregion
    
    /// <summary>
    /// Converts a Size structure to a SizeF structure.
    /// </summary>
    /// <param name="size">The size to convert.</param>
    public static SizeF ToSizeF(Size size) => new(size.Width, size.Height);

    #region Equality Methods
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Size other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Size other) => Width == other.Width && Height == other.Height;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    /// Determines whether two sizes are equal.
    /// </summary>
    public static bool operator ==(Size left, Size right) => left.Equals(right);

    /// <summary>
    /// Determines whether two sizes are not equal.
    /// </summary>
    public static bool operator !=(Size left, Size right) => !left.Equals(right);
    #endregion

    #region Comparisonz methods
    public int CompareTo(Size other)
    {
        var widthComparison = Width.CompareTo(other.Width);
        return widthComparison is not 0 ? widthComparison : Height.CompareTo(other.Height);
    }

    public int CompareTo(object? obj) => ReferenceEquals(null, obj) ? 1 :
        obj is Size other ? CompareTo(other) :
            throw new ArgumentException($"Object must be of type {nameof(Size)}");

    public static bool operator <(Size left, Size right) => left.CompareTo(right) < 0;

    public static bool operator >(Size left, Size right) => left.CompareTo(right) > 0;

    public static bool operator <=(Size left, Size right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Size left, Size right) => left.CompareTo(right) >= 0;
    #endregion

    /// <summary>
    /// Deconstructs the size into its width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public void Deconstruct(out int width, out int height)
    {
        width = Width;
        height = Height;
    }
}