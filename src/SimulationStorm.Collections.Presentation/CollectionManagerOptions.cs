using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Primitives;

namespace SimulationStorm.Collections.Presentation;

public class CollectionManagerOptions : ICollectionManagerOptions
{
    public bool IsSavingEnabled { get; init; }
    
    public int SavingInterval { get; init; }
    
    public CollectionStorageLocation StorageLocation { get; init; }
    
    public int Capacity { get; init; }

    public Range<int> CapacityRange { get; init; }
    
    public Range<int> SavingIntervalRange { get; init; }
}