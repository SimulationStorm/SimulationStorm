using System.Collections.Generic;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Presentation;

public abstract class CollectionAndManagerSaveBase<T>
{
    public bool IsSavingEnabled { get; init; }

    public int SavingInterval { get; init; }
    
    public CollectionStorageLocation StorageLocation { get; init; }

    public int Capacity { get; init; }

    public IEnumerable<T> Items { get; init; } = null!;
}