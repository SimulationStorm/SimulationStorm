using System.Collections.Generic;
using System.Collections.Specialized;
using SimulationStorm.Collections.Pointed;
using SimulationStorm.Collections.Round;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Universal;

/// <summary>
/// Represents a read-only variant of a collection that provides many different possibilities.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IReadOnlyUniversalCollection<out T> :
    IReadOnlyList<T>,
    IReadOnlyStorageControlCollection,
    IReadOnlyRoundCollection,
    INotifyCollectionChanged,
    IReadOnlyPointedCollection;