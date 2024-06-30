using System;

namespace SimulationStorm.Collections.Pointed;

/// <summary>
/// Provides data for the <see cref="IPointedCollection.PointerMoved"/> event.
/// </summary>
/// <param name="oldPosition">The old pointer position.</param>
/// <param name="newPosition">The new pointer position.</param>
public class CollectionPointerMovedEventArgs(int oldPosition, int newPosition) : EventArgs
{
    /// <summary>
    /// Gets the old pointer position.
    /// </summary>
    public int OldPosition { get; } = oldPosition;

    /// <summary>
    /// Gets the new pointer position.
    /// </summary>
    public int NewPosition { get; } = newPosition;

    /// <summary>
    /// Gets the absolute difference between old and new pointer positions.
    /// </summary>
    public int PositionDelta => Math.Abs(OldPosition - NewPosition);

    /// <summary>
    /// Gets the direction in which the pointer has moved.
    /// </summary>
    public PointerMovementDirection MovementDirection => NewPosition < OldPosition
        ? PointerMovementDirection.Backward
        : PointerMovementDirection.Forward;
}