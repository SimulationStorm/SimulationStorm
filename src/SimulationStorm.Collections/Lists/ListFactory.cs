using System;
using System.Collections.Generic;
using DynamicData;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Lists;

public class ListFactory : IListFactory
{
    public IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation) => storageLocation switch
    {
        CollectionStorageLocation.Memory => new ExtendedList<T>(),
        CollectionStorageLocation.FileSystem => new FileSystemList<T>(),
        _ => throw new ArgumentException("Invalid storage type.", nameof(storageLocation))
    };

    public IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation, int capacity) => storageLocation switch
    {
        CollectionStorageLocation.Memory => new ExtendedList<T>(capacity),
        CollectionStorageLocation.FileSystem => new FileSystemList<T>(capacity),
        _ => throw new ArgumentException("Invalid storage type.", nameof(storageLocation))
    };

    public IExtendedList<T> CreateList<T>(CollectionStorageLocation storageLocation, IEnumerable<T> collection) => storageLocation switch
    {
        CollectionStorageLocation.Memory => new ExtendedList<T>(collection),
        CollectionStorageLocation.FileSystem => new FileSystemList<T>(collection),
        _ => throw new ArgumentException("Invalid storage type.", nameof(storageLocation))
    };
}