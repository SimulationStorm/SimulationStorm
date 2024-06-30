namespace SimulationStorm.Collections.StorageControl;

/// <summary>
/// Represents a read-only variant of a collection that can change the physical location where items are stored.
/// </summary>
public interface IReadOnlyStorageControlCollection
{
    /// <summary>
    /// Gets the current items storage location.
    /// </summary>
    CollectionStorageLocation StorageLocation { get; }
}