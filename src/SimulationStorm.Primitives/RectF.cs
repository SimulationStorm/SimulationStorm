using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents a rectangle with floating point coordinates.
/// </summary>
public readonly struct RectF : IEquatable<RectF>
{
    #region Properties
    /// <summary>
    /// Gets the left coordinate of the rectangle.
    /// </summary>
    public float Left { get; }

    /// <summary>
    /// Gets the top coordinate of the rectangle.
    /// </summary>
    public float Top { get; }

    /// <summary>
    /// Gets the right coordinate of the rectangle.
    /// </summary>
    public float Right { get; }

    /// <summary>
    /// Gets the bottom coordinate of the rectangle.
    /// </summary>
    public float Bottom { get; }

    /// <summary>
    /// Gets the position of the top-left corner of the rectangle.
    /// </summary>
    public PointF Position => new(Left, Top);

    /// <summary>
    /// Gets the position of the bottom-right corner of the rectangle.
    /// </summary>
    public PointF End => new(Right, Bottom);

    /// <summary>
    /// Gets the center of the rectangle.
    /// </summary>
    public PointF Center => new((Left + Right) / 2, (Top + Bottom) / 2);
    #endregion

    /// <param name="left">The left coordinate of the rectangle.</param>
    /// <param name="top">The top coordinate of the rectangle.</param>
    /// <param name="right">The right coordinate of the rectangle.</param>
    /// <param name="bottom">The bottom coordinate of the rectangle.</param>
    [JsonConstructor]
    public RectF(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    
    #region Methods
    /// <summary>
    /// Determines whether the rectangle contains the specified pofloat.
    /// </summary>
    /// <param name="point">The pofloat to check.</param>
    /// <returns>True if the rectangle contains the point; otherwise, false.</returns>
    public bool Contains(PointF point) =>
        point.X >= Left &&
        point.Y >= Top &&
        point.X <= Right &&
        point.Y <= Bottom;

    /// <summary>
    /// Converts the rectangle to a <see cref="Rect"/> (rectangle with integer coordinates).
    /// </summary>
    /// <returns>A <see cref="Rect"/> representation of the rectangle.</returns>
    public Rect ToRect() => new((int)Left, (int)Top, (int)Right, (int)Bottom);
    #endregion

    #region Equality methods
    /// <inheritdoc/>
    public bool Equals(RectF other) =>
        Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right) && Bottom.Equals(other.Bottom);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is RectF other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

    /// <summary>
    /// Determines whether two rectangles are equal.
    /// </summary>
    public static bool operator ==(RectF left, RectF right) => left.Equals(right);

    /// <summary>
    /// Determines whether two rectangles are not equal.
    /// </summary>
    public static bool operator !=(RectF left, RectF right) => !left.Equals(right);
    #endregion

    public void Deconstruct(out float left, out float top, out float right, out float bottom)
    {
        left = Left;
        top = Top;
        right = Right;
        bottom = Bottom;
    }
}