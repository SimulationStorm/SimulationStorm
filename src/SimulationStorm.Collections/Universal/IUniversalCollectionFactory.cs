using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Universal;

public interface IUniversalCollectionFactory
{
    IUniversalCollection<T> Create<T>(CollectionStorageLocation storageLocation, int capacity);
}