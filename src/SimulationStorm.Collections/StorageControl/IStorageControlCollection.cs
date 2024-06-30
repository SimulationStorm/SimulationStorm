namespace SimulationStorm.Collections.StorageControl;

/// <summary>
/// Represents a collection that can change the physical location where items are stored.
/// </summary>
public interface IStorageControlCollection : IReadOnlyStorageControlCollection
{
    /// <summary>
    /// Gets or sets the items storage location.
    /// </summary>
    new CollectionStorageLocation StorageLocation { get; set; }
}