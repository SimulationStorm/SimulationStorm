using System;

namespace SimulationStorm.Collections.Pointed;

/// <summary>
/// Represents a read-only variant of a collection that has a position/index pointer that can be moved through the collection.
/// </summary>
public interface IReadOnlyPointedCollection
{
    /// <summary>
    /// Gets the current pointer position.
    /// The minimum value is -1 (the pointer does not point to any item)
    /// and the maximum value is the index of the last item.
    /// </summary>
    int PointerPosition { get; }
    
    /// <summary>
    /// Occurs when the pointer position changes.
    /// </summary>
    event EventHandler<CollectionPointerMovedEventArgs>? PointerMoved;
}