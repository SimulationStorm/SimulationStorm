using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents a rectangle with integer coordinates.
/// </summary>
public readonly struct Rect : IEquatable<Rect>
{
    #region Properties
    /// <summary>
    /// Gets the left coordinate of the rectangle.
    /// </summary>
    public int Left { get; }

    /// <summary>
    /// Gets the top coordinate of the rectangle.
    /// </summary>
    public int Top { get; }

    /// <summary>
    /// Gets the right coordinate of the rectangle.
    /// </summary>
    public int Right { get; }

    /// <summary>
    /// Gets the bottom coordinate of the rectangle.
    /// </summary>
    public int Bottom { get; }

    /// <summary>
    /// Gets the position of the top-left corner of the rectangle.
    /// </summary>
    public Point Position => new(Left, Top);

    /// <summary>
    /// Gets the position of the bottom-right corner of the rectangle.
    /// </summary>
    public Point End => new(Right, Bottom);

    /// <summary>
    /// Gets the center of the rectangle.
    /// </summary>
    public Point Center => new((Left + Right) / 2, (Top + Bottom) / 2);
    #endregion

    /// <param name="left">The left coordinate of the rectangle.</param>
    /// <param name="top">The top coordinate of the rectangle.</param>
    /// <param name="right">The right coordinate of the rectangle.</param>
    /// <param name="bottom">The bottom coordinate of the rectangle.</param>
    [JsonConstructor]
    public Rect(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    
    #region Methods
    /// <summary>
    /// Determines whether the rectangle contains the specified point.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the rectangle contains the point; otherwise, false.</returns>
    public bool Contains(Point point) =>
        point.X >= Left &&
        point.Y >= Top &&
        point.X <= Right &&
        point.Y <= Bottom;

    /// <summary>
    /// Converts the rectangle to a <see cref="RectF"/> (rectangle with float coordinates).
    /// </summary>
    /// <returns>A <see cref="RectF"/> representation of the rectangle.</returns>
    public RectF ToRectF() => new(Left, Top, Right, Bottom);
    #endregion

    #region Equality methods
    /// <inheritdoc/>
    public bool Equals(Rect other) =>
        Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Rect other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

    /// <summary>
    /// Determines whether two rectangles are equal.
    /// </summary>
    public static bool operator ==(Rect left, Rect right) => left.Equals(right);

    
    /// <summary>
    /// Determines whether two rectangles are not equal.
    /// </summary>
    public static bool operator !=(Rect left, Rect right) => !left.Equals(right);
    #endregion

    public void Deconstruct(out int left, out int top, out int right, out int bottom)
    {
        left = Left;
        top = Top;
        right = Right;
        bottom = Bottom;
    }
}