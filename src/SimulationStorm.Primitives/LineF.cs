using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents a line segment defined by its start and end points with floating point coordinates.
/// </summary>
public readonly struct LineF : IEquatable<LineF>
{
    #region Properties
    /// <summary>
    /// Gets the X-coordinate of the start point of the line.
    /// </summary>
    public float StartX { get; }

    /// <summary>
    /// Gets the Y-coordinate of the start point of the line.
    /// </summary>
    public float StartY { get; }

    /// <summary>
    /// Gets the X-coordinate of the end point of the line.
    /// </summary>
    public float EndX { get; }

    /// <summary>
    /// Gets the Y-coordinate of the end point of the line.
    /// </summary>
    public float EndY { get; }

    /// <summary>
    /// Gets the start point of the line.
    /// </summary>
    public PointF Start => new(StartX, StartY);

    /// <summary>
    /// Gets the end point of the line.
    /// </summary>
    public PointF End => new(EndX, EndY);
    #endregion
 
    /// <param name="startX">The X-coordinate of the start point of the line.</param>
    /// <param name="startY">The Y-coordinate of the start point of the line.</param>
    /// <param name="endX">The X-coordinate of the end point of the line.</param>
    /// <param name="endY">The Y-coordinate of the end point of the line.</param>
    [JsonConstructor]
    public LineF(float startX, float startY, float endX, float endY)
    {
        StartX = startX;
        StartY = startY;
        EndX = endX;
        EndY = endY;
    }
    
    /// <summary>
    /// Converts the line to a <see cref="Line"/> (line with integer coordinates).
    /// </summary>
    /// <returns>A <see cref="Line"/> representation of the line.</returns>
    public Line ToLine() => new((int)StartX, (int)StartY, (int)EndX, (int)EndY);

    #region Equality methods
    /// <inheritdoc/>
    public bool Equals(LineF other) =>
        StartX.Equals(other.StartX) && StartY.Equals(other.StartY) && EndX.Equals(other.EndX) && EndY.Equals(other.EndY);
    
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is LineF other && Equals(other);
    
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(StartX, StartY, EndX, EndY);
    
    /// <summary>
    /// Determines whether two lines are equal.
    /// </summary>
    /// <param name="left">The first line.</param>
    /// <param name="right">The second line.</param>
    /// <returns>True if the lines are equal; otherwise, false.</returns>
    
    public static bool operator ==(LineF left, LineF right) => left.Equals(right);
    /// <summary>
    /// Determines whether two lines are not equal.
    /// </summary>
    /// <param name="left">The first line.</param>
    /// <param name="right">The second line.</param>
    /// <returns>True if the lines are not equal; otherwise, false.</returns>
    public static bool operator !=(LineF left, LineF right) => !left.Equals(right);
    #endregion

    /// <summary>
    /// Deconstructs the line into its constituent parts.
    /// </summary>
    /// <param name="startX">The X-coordinate of the start point of the line.</param>
    /// <param name="startY">The Y-coordinate of the start point of the line.</param>
    /// <param name="endX">The X-coordinate of the end point of the line.</param>
    /// <param name="endY">The Y-coordinate of the end point of the line.</param>
    public void Deconstruct(out float startX, out float startY, out float endX, out float endY)
    {
        startX = StartX;
        startY = StartY;
        endX = EndX;
        endY = EndY;
    }
}