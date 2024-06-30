using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents an immutable 2D point with floating-point coordinates.
/// </summary>
public readonly struct PointF : IEquatable<PointF>, IComparable<PointF>, IComparable
{
    /// <summary>
    /// Represents a point with coordinates (0, 0).
    /// </summary>
    public static readonly PointF Zero = new(0, 0);

    /// <summary>
    /// Represents a point with coordinates (1, 1).
    /// </summary>
    public static readonly PointF One = new(1, 1);

    #region Properties
    /// <summary>
    /// Gets the X-coordinate of the point.
    /// </summary>
    public float X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the point.
    /// </summary>
    public float Y { get; }
    #endregion

    /// <param name="x">The X-coordinate of the point.</param>
    /// <param name="y">The Y-coordinate of the point.</param>
    [JsonConstructor]
    public PointF(float x, float y)
    {
        X = x;
        Y = y;
    }

    #region Operators
    /// <summary>
    /// Negates the specified pointF.
    /// </summary>
    public static PointF operator -(PointF point) => new(-point.X, -point.Y);

    /// <summary>
    /// Adds two points together.
    /// </summary>
    public static PointF operator +(PointF left, PointF right) => new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts one point from another.
    /// </summary>
    public static PointF operator -(PointF left, PointF right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies two points component-wise.
    /// </summary>
    public static PointF operator *(PointF left, PointF right) => new(left.X * right.X, left.Y * right.Y);

    /// <summary>
    /// Divides one point by another component-wise.
    /// </summary>
    public static PointF operator /(PointF left, PointF right) => new(left.X / right.X, left.Y / right.Y);

    /// <summary>
    /// Adds a float number to each component of the pointF.
    /// </summary>
    public static PointF operator +(PointF point, float number) => new(point.X + number, point.Y + number);

    /// <summary>
    /// Subtracts a float number from each component of the pointF.
    /// </summary>
    public static PointF operator -(PointF point, float number) => new(point.X - number, point.Y - number);

    /// <summary>
    /// Multiplies each component of the pointF by a float number.
    /// </summary>
    public static PointF operator *(PointF point, float number) => new(point.X * number, point.Y * number);

    /// <summary>
    /// Divides each component of the pointF by a float number.
    /// </summary>
    public static PointF operator /(PointF point, float number) => new(point.X / number, point.Y / number);
    #endregion
    
    /// <summary>
    /// Converts a PointF structure to a Point structure by rounding its components to the nearest integer values.
    /// </summary>
    public Point ToPoint() => new((int)X, (int)Y);

    #region Equality Methods
    /// <inheritdoc />
    public bool Equals(PointF other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is PointF other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two points are equal.
    /// </summary>
    public static bool operator ==(PointF left, PointF right) => left.Equals(right);

    /// <summary>
    /// Determines whether two points are not equal.
    /// </summary>
    public static bool operator !=(PointF left, PointF right) => !left.Equals(right);
    #endregion
    
    #region Comparison methods
    public int CompareTo(PointF other)
    {
        var xComparison = X.CompareTo(other.X);
        return xComparison is not 0 ? xComparison : Y.CompareTo(other.Y);
    }

    public int CompareTo(object? obj) => ReferenceEquals(null, obj) ? 1 :
        obj is PointF other ? CompareTo(other) :
        throw new ArgumentException($"Object must be of type {nameof(PointF)}");

    public static bool operator <(PointF left, PointF right) => left.CompareTo(right) < 0;

    public static bool operator >(PointF left, PointF right) => left.CompareTo(right) > 0;

    public static bool operator <=(PointF left, PointF right) => left.CompareTo(right) <= 0;

    public static bool operator >=(PointF left, PointF right) => left.CompareTo(right) >= 0;
    #endregion

    /// <summary>
    /// Deconstructs the point into its X and Y coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate of the point.</param>
    /// <param name="y">The Y-coordinate of the point.</param>
    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }
}