using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

/// <summary>
/// Represents a line segment defined by its start and end points with integer coordinates.
/// </summary>
public readonly struct Line : IEquatable<Line>
{
    #region Properties
    /// <summary>
    /// Gets the X-coordinate of the start point of the line.
    /// </summary>
    public int StartX { get; }

    /// <summary>
    /// Gets the Y-coordinate of the start point of the line.
    /// </summary>
    public int StartY { get; }

    /// <summary>
    /// Gets the X-coordinate of the end point of the line.
    /// </summary>
    public int EndX { get; }

    /// <summary>
    /// Gets the Y-coordinate of the end point of the line.
    /// </summary>
    public int EndY { get; }

    /// <summary>
    /// Gets the start point of the line.
    /// </summary>
    public Point Start => new(StartX, StartY);

    /// <summary>
    /// Gets the end point of the line.
    /// </summary>
    public Point End => new(EndX, EndY);
    #endregion
 
    /// <param name="startX">The X-coordinate of the start point of the line.</param>
    /// <param name="startY">The Y-coordinate of the start point of the line.</param>
    /// <param name="endX">The X-coordinate of the end point of the line.</param>
    /// <param name="endY">The Y-coordinate of the end point of the line.</param>
    [JsonConstructor]
    public Line(int startX, int startY, int endX, int endY)
    {
        StartX = startX;
        StartY = startY;
        EndX = endX;
        EndY = endY;
    }
    
    /// <summary>
    /// Converts the line to a <see cref="LineF"/> (line with float coordinates).
    /// </summary>
    /// <returns>A <see cref="LineF"/> representation of the line.</returns>
    public LineF ToLineF() => new(StartX, StartY, EndX, EndY);

    #region Equality methods
    /// <inheritdoc/>
    public bool Equals(Line other) =>
        StartX == other.StartX && StartY == other.StartY && EndX == other.EndX && EndY == other.EndY;
    
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Line other && Equals(other);
    
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(StartX, StartY, EndX, EndY);
    
    /// <summary>
    /// Determines whether two lines are equal.
    /// </summary>
    /// <param name="left">The first line.</param>
    /// <param name="right">The second line.</param>
    /// <returns>True if the lines are equal; otherwise, false.</returns>
    
    public static bool operator ==(Line left, Line right) => left.Equals(right);
    /// <summary>
    /// Determines whether two lines are not equal.
    /// </summary>
    /// <param name="left">The first line.</param>
    /// <param name="right">The second line.</param>
    /// <returns>True if the lines are not equal; otherwise, false.</returns>
    public static bool operator !=(Line left, Line right) => !left.Equals(right);
    #endregion

    /// <summary>
    /// Deconstructs the line into its constituent parts.
    /// </summary>
    /// <param name="startX">The X-coordinate of the start point of the line.</param>
    /// <param name="startY">The Y-coordinate of the start point of the line.</param>
    /// <param name="endX">The X-coordinate of the end point of the line.</param>
    /// <param name="endY">The Y-coordinate of the end point of the line.</param>
    public void Deconstruct(out int startX, out int startY, out int endX, out int endY)
    {
        startX = StartX;
        startY = StartY;
        endX = EndX;
        endY = EndY;
    }
}