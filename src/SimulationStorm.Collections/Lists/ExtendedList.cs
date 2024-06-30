using System.Collections.Generic;
using DynamicData;

namespace SimulationStorm.Collections.Lists;

public class ExtendedList<T> : List<T>, IExtendedList<T>
{
    #region Constructors
    /// <inheritdoc/>
    public ExtendedList() { }

    /// <inheritdoc/>
    public ExtendedList(int capacity) : base(capacity) { }
    
    /// <inheritdoc/>
    public ExtendedList(IEnumerable<T> collection) : base(collection) { }
    #endregion

    /// <inheritdoc/>
    public void InsertRange(IEnumerable<T> collection, int index) => InsertRange(index, collection);

    /// <inheritdoc/>
    public void Move(int original, int destination)
    {
        var item = this[original];
        RemoveAt(original);
        Insert(destination, item);
    }
}