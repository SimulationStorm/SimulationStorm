using System.Collections.Generic;
using DynamicData;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Lists;

public interface IListFactory
{
    IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation);
    
    IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation, int capacity);
    
    IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation, IEnumerable<T> collection);
}