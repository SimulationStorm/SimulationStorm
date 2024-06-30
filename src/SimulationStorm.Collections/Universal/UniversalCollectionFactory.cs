using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Universal;

public class UniversalCollectionFactory(IListFactory listFactory) : IUniversalCollectionFactory
{
    public IUniversalCollection<T> Create<T>(CollectionStorageLocation storageLocation, int capacity) =>
        new UniversalCollection<T>(listFactory, storageLocation, capacity);
}