using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Primitives;

namespace SimulationStorm.Collections.Presentation;

public interface ICollectionManagerOptions
{
    bool IsSavingEnabled { get; }
    
    int SavingInterval { get; }
    
    CollectionStorageLocation StorageLocation { get; }
    
    int Capacity { get; }

    Range<int> SavingIntervalRange { get; }

    Range<int> CapacityRange { get; }
}