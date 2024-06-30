using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents an immutable 2D point with integer coordinates.
/// </summary>
public readonly struct Point : IEquatable<Point>, IComparable<Point>, IComparable
{
    /// <summary>
    /// Represents a point with coordinates (0, 0).
    /// </summary>
    public static readonly Point Zero = new(0, 0);

    /// <summary>
    /// Represents a point with coordinates (1, 1).
    /// </summary>
    public static readonly Point One = new(1, 1);

    #region Properties
    /// <summary>
    /// Gets the X-coordinate of the point.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the point.
    /// </summary>
    public int Y { get; }
    #endregion
    
    /// <param name="x">The X-coordinate of the point.</param>
    /// <param name="y">The Y-coordinate of the point.</param>
    [JsonConstructor]
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    #region Operators
    /// <summary>
    /// Negates the specified point.
    /// </summary>
    public static Point operator -(Point point) => new(-point.X, -point.Y);

    /// <summary>
    /// Adds two points together.
    /// </summary>
    public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts one point from another.
    /// </summary>
    public static Point operator -(Point left, Point right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies two points component-wise.
    /// </summary>
    public static Point operator *(Point left, Point right) => new(left.X * right.X, left.Y * right.Y);

    /// <summary>
    /// Divides one point by another component-wise.
    /// </summary>
    public static Point operator /(Point left, Point right) => new(left.X / right.X, left.Y / right.Y);

    /// <summary>
    /// Adds an integer number to each component of the point.
    /// </summary>
    public static Point operator +(Point point, int number) => new(point.X + number, point.Y + number);

    /// <summary>
    /// Subtracts an integer number from each component of the point.
    /// </summary>
    public static Point operator -(Point point, int number) => new(point.X - number, point.Y - number);

    /// <summary>
    /// Multiplies each component of the point by an integer number.
    /// </summary>
    public static Point operator *(Point point, int number) => new(point.X * number, point.Y * number);

    /// <summary>
    /// Divides each component of the point by an integer number.
    /// </summary>
    public static Point operator /(Point point, int number) => new(point.X / number, point.Y / number);
    #endregion
    
    /// <summary>
    /// Converts a Point structure to a PointF structure.
    /// </summary>
    public PointF ToPointF() => new(X, Y);

    #region Equality Methods
    /// <inheritdoc />
    public bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Point other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two points are equal.
    /// </summary>
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    /// <summary>
    /// Determines whether two points are not equal.
    /// </summary>
    public static bool operator !=(Point left, Point right) => !left.Equals(right);
    #endregion
    
    #region Comparison methods
    public int CompareTo(Point other)
    {
        var xComparison = X.CompareTo(other.X);
        return xComparison is not 0 ? xComparison : Y.CompareTo(other.Y);
    }

    public int CompareTo(object? obj) => ReferenceEquals(null, obj) ? 1 :
        obj is Point other ? CompareTo(other) :
        throw new ArgumentException($"Object must be of type {nameof(Point)}");

    public static bool operator <(Point left, Point right) => left.CompareTo(right) < 0;

    public static bool operator >(Point left, Point right) => left.CompareTo(right) > 0;

    public static bool operator <=(Point left, Point right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Point left, Point right) => left.CompareTo(right) >= 0;
    #endregion

    /// <summary>
    /// Deconstructs the point into its X and Y coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate of the point.</param>
    /// <param name="y">The Y-coordinate of the point.</param>
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}