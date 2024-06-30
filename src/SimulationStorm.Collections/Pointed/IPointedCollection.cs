namespace SimulationStorm.Collections.Pointed;

/// <summary>
/// Represents a collection that has a position/index pointer that can be moved through the collection.
/// </summary>
public interface IPointedCollection : IReadOnlyPointedCollection
{
    /// <summary>
    /// Gets or sets the pointer behavior.
    /// </summary>
    PointerBehavior PointerBehavior { get; set; }
    
    /// <summary>
    /// Moves the pointer to the <see cref="newPosition"/> and raises the <see cref="IReadOnlyPointedCollection.PointerMoved"/> event.
    /// </summary>
    /// <param name="newPosition">The new pointer position.</param>
    void MovePointer(int newPosition);
}