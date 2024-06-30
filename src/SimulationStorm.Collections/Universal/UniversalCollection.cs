using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DynamicData;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.Pointed;
using SimulationStorm.Collections.StorageControl;

namespace SimulationStorm.Collections.Universal;

// Todo: Implement reentrancy, as in ObservableCollection.
// What about concurrency - it is difficult and is not required here.
// Todo: implement suspendable notify collection changed; make pointer moved event also suspendable

public class UniversalCollection<T> : IUniversalCollection<T>
{
    private const int DefaultPointerPosition = -1;

    #region Properties
    public int Count => _listStorage.Count; //- _hiddenRanges.Sum(x => x.Length());

    public T this[int index]
    {
        get => _listStorage[index];
        set => _listStorage[index] = value;
    }

    public CollectionStorageLocation StorageLocation
    {
        get => _storageLocation;
        set => SetStorageLocation(value);
    }

    public int Capacity
    {
        get => _capacity;
        set => SetCapacity(value);
    }

    public int PointerPosition { get; private set; } = DefaultPointerPosition;

    // Todo: For now, set this property here.
    public PointerBehavior PointerBehavior { get; set; } =
        PointerBehavior.RemoveItemsAheadOfPointerBeforeAddingOrInsertingItems
        | PointerBehavior.MoveToEndAfterAddingOrInsertingItems;

    public bool IsReadOnly => false;
    
    public bool UseRemoveActionOverReset { get; set; }
    
    int ICollection<T>.Count => _listStorage.Count;

    int IReadOnlyCollection<T>.Count => _listStorage.Count;
    #endregion

    #region Events
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<CollectionPointerMovedEventArgs>? PointerMoved;
    #endregion

    #region Fields
    private readonly IListFactory _listFactory;

    private IExtendedList<T> _listStorage;

    private CollectionStorageLocation _storageLocation;
    
    private int _capacity;

    // private readonly IList<Range> _hiddenRanges = new List<Range>();
    #endregion

    public UniversalCollection(IListFactory listFactory, CollectionStorageLocation storageLocation, int capacity)
    {
        _listFactory = listFactory;

        _storageLocation = storageLocation;
        
        ValidateCapacity(capacity);
        _capacity = capacity;
    
        _listStorage = _listFactory.CreateList<T>(StorageLocation, Capacity);
    }

    #region Public methods
    #region Adding methods
    public void Add(T item) => Insert(Count, item);

    public void AddRange(IEnumerable<T> collection) => InsertRange(collection, Count);
    
    public void Insert(int index, T item)
    {
        var oldCount = Count;
        
        RemoveItemsAfterPointerIfThereAreIfNeeded(out var removedItemCount);
        index -= removedItemCount;

        EnsureThereIsRequiredSpace(1, out removedItemCount);
        index -= removedItemCount;
        
        _listStorage.Insert(index, item);
        
        NotifyCountChangedIfChanged(oldCount);
        
        NotifyCollectionChanged
        (
            new NotifyCollectionChangedEventArgs
            (
                NotifyCollectionChangedAction.Add,
                item,
                index
            )
        );
        
        MovePointerToEndIfNeeded();
    }

    public void InsertRange(IEnumerable<T> collection, int index)
    {
        var oldCount = Count;
        
        var itemsToInsert = collection
            .TakeLast(Capacity)
            .ToList();
        
        RemoveItemsAfterPointerIfThereAreIfNeeded(out var removedItemCount);
        index -= removedItemCount;
        
        EnsureThereIsRequiredSpace(itemsToInsert.Count, out removedItemCount);
        index -= removedItemCount;
        
        _listStorage.InsertRange(itemsToInsert, index);
        
        NotifyCountChangedIfChanged(oldCount);
        
        NotifyCollectionChanged
        (
            new NotifyCollectionChangedEventArgs
            (
                NotifyCollectionChangedAction.Add,
                itemsToInsert,
                index
            )
        );
        
        MovePointerToEndIfNeeded();
    }
    #endregion

    #region Removing methods
    public bool Remove(T item)
    {
        var index = _listStorage.IndexOf(item);
        if (index is -1)
            return false;

        RemoveAt(index);
        
        return true;
    }

    public void RemoveAt(int index)
    {
        var oldCount = Count;
        
        var itemToRemove = _listStorage[index];
        
        _listStorage.RemoveAt(index);

        CheckPointerPositionAndMoveIfNeeded();
        
        NotifyCountChangedIfChanged(oldCount);
        
        NotifyCollectionChanged
        (
            new NotifyCollectionChangedEventArgs
            (
                NotifyCollectionChangedAction.Remove,
                itemToRemove,
                index
            )
        );
    }

    public void RemoveRange(int index, int count)
    {
        var oldCount = Count;

        var itemsToRemove = new List<T>(count);
        for (var i = index; i < index + count; i++)
            itemsToRemove.Add(_listStorage[i]);

        _listStorage.RemoveRange(index, count);
        
        CheckPointerPositionAndMoveIfNeeded();
        
        NotifyCountChangedIfChanged(oldCount);

        NotifyCollectionChanged
        (
            new NotifyCollectionChangedEventArgs
            (
                NotifyCollectionChangedAction.Remove,
                itemsToRemove,
                index
            )
        );
    }

    public void Clear()
    {
        if (UseRemoveActionOverReset)
            ClearUsingRemoveRange();
        else
            ClearUsingClear();
    }
    #endregion

    public void Move(int original, int destination)
    {
        var item = _listStorage[original];
        
        _listStorage.RemoveAt(original);
        _listStorage.Insert(destination, item);
        
        NotifyCollectionChanged
        (
            new NotifyCollectionChangedEventArgs
            (
                NotifyCollectionChangedAction.Move,
                item,
                destination,
                original
            )
        );
    }

    #region Searching methods
    public bool Contains(T item) => _listStorage.Contains(item);

    public int IndexOf(T item) => _listStorage.IndexOf(item);
    #endregion

    public void MovePointer(int newPosition)
    {
        ValidatePointerPosition(newPosition);
        
        var oldPosition = PointerPosition;
        if (newPosition == oldPosition)
            return;
        
        PointerPosition = newPosition;
        NotifyPointerMoved(oldPosition);
    }

    // #region Visibility changing methods
    // // Here, it also needed to create custom enumerator.
    // // And permit collection modifying when there are hidden items.
    // public void HideRange(int index, int count)
    // {
    //     // Check if index already hidden
    //     var endIndex = index + count;
    //     if (_hiddenRanges.Any(r => r.StartIndex() >= index || r.EndIndex() <= endIndex))
    //         ; // is already hidden
    //     
    //     _hiddenRanges.Add(new Range(new Index(index), new Index(index + count)));
    //
    //     var itemsToHide = _listStorage.Skip(index).Take(count);
    //     NotifyCollectionChanged
    //     (
    //         new NotifyCollectionChangedEventArgs
    //         (
    //             NotifyCollectionChangedAction.Remove,
    //             itemsToHide,
    //             index
    //         )
    //     );
    // }
    //
    // public void RevealRange(int index, int count)
    // {
    // }
    //
    // public void RemoveHidden()
    // {
    //     // Take all hidden items and remove them.
    //     // var hiddenItems = ;
    // }
    // #endregion
    
    public void CopyTo(T[] array, int arrayIndex) => _listStorage.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _listStorage.GetEnumerator();
    #endregion

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Private methods
    #region Storage location setting
    private void SetStorageLocation(CollectionStorageLocation newStorageLocation)
    {
        var newStorage = _listFactory.CreateList<T>(newStorageLocation, Count);
        
        for (var i = 0; i < Count; i++)
            newStorage.Add(_listStorage[i]);
        
        _listStorage.Clear();
        _listStorage = newStorage;

        _storageLocation = newStorageLocation;
        NotifyStorageLocationChanged();
    }
    
    private void NotifyStorageLocationChanged() => NotifyPropertyChanged(nameof(StorageLocation));
    #endregion

    #region Capacity setting
    private void SetCapacity(int newCapacity)
    {
        ValidateCapacity(newCapacity);

        if (Count > newCapacity)
        {
            var itemCountToRemove = Count - newCapacity;
            RemoveRange(0, itemCountToRemove);
        }
        
        _capacity = newCapacity;
        NotifyCapacityChanged();
    }
    
    private static void ValidateCapacity(int capacity) =>
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1, nameof(capacity));

    private void NotifyCapacityChanged() => NotifyPropertyChanged(nameof(Capacity));
    #endregion
    
    #region Pointer movement
    private void MovePointerToEndIfNeeded()
    {
        if (PointerBehavior.HasFlag(PointerBehavior.MoveToEndAfterAddingOrInsertingItems))
            MovePointerToEnd();
    }

    private void MovePointerToEnd() => MovePointer(GetMaxIndex());
    
    private void CheckPointerPositionAndMoveIfNeeded()
    {
        var maxItemIndex = GetMaxIndex();
        if (PointerPosition > maxItemIndex)
            MovePointer(maxItemIndex);
    }
    
    private void ValidatePointerPosition(int pointerPosition)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pointerPosition, DefaultPointerPosition, nameof(pointerPosition));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(pointerPosition, GetMaxIndex(), nameof(pointerPosition));
    }
    #endregion

    #region Removing items after pointer
    private void RemoveItemsAfterPointerIfThereAreIfNeeded(out int removedItemCount)
    {
        removedItemCount = 0;
        
        if (PointerBehavior.HasFlag(PointerBehavior.RemoveItemsAheadOfPointerBeforeAddingOrInsertingItems))
            RemoveItemsAfterPointerIfThereAre(out removedItemCount);
    }
    
    private void RemoveItemsAfterPointerIfThereAre(out int removedItemCount)
    {
        removedItemCount = GetMaxIndex() - PointerPosition;
        if (removedItemCount > 0)
            RemoveRange(PointerPosition + 1, removedItemCount);
    }
    #endregion

    #region Notification methods
    private void NotifyCountChangedIfChanged(int oldCount)
    {
        if (Count != oldCount)
            NotifyPropertyChanged(nameof(Count));
    }

    private void NotifyPointerMoved(int oldPosition) =>
        PointerMoved?.Invoke(this, new CollectionPointerMovedEventArgs(oldPosition, PointerPosition));
    
    private void NotifyPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);
    #endregion

    #region Clearing methods
    private void ClearUsingRemoveRange() => RemoveRange(0, Count);

    private void ClearUsingClear()
    {
        var oldCount = Count;
        
        _listStorage.Clear();
            
        MovePointer(DefaultPointerPosition);
        
        NotifyCountChangedIfChanged(oldCount);
        
        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    #endregion
    
    private void EnsureThereIsRequiredSpace(int requiredSpace, out int removedItemCount)
    {
        var availableSpace = GetAvailableSpace();
        if (availableSpace >= requiredSpace)
        {
            removedItemCount = 0;
            return;
        }
        
        removedItemCount = requiredSpace - availableSpace;
        RemoveRange(0, removedItemCount);
    }
    
    private int GetMaxIndex() => Count - 1;
    
    private int GetAvailableSpace() => Capacity - Count;
    #endregion
}