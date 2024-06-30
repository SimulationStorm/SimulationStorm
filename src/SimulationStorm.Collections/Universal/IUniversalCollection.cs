using DynamicData;
using SimulationStorm.Collections.Pointed;
using SimulationStorm.Collections.Round;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Universal;

/// <summary>
/// Represents a collection that provides many different possibilities.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IUniversalCollection<T> :
    IReadOnlyUniversalCollection<T>,
    IExtendedList<T>,
    IStorageControlCollection,
    IRoundCollection,
    IExtendedNotifyCollectionChanged,
    IPointedCollection
{
    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    new int Count { get; }
    
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
    new T this[int index] { get; set; }
}