using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DynamicData;

namespace SimulationStorm.Collections.Lists;

/// <summary>
/// Represents a strongly typed list of objects which are stored in the file system and can be accessed by index.
/// Provides methods to search, sort, and manipulate lists.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class FileSystemList<T> : IReadOnlyList<T>, IExtendedList<T>
{
    #region Properties
    /// <summary>
    /// Gets the number of elements contained in the <see cref="FileSystemList{T}" />.
    /// </summary>
    public int Count => _itemFilePaths.Count;

    /// <inheritdoc/>
    int ICollection<T>.Count => _itemFilePaths.Count;

    /// <inheritdoc/>
    int IReadOnlyCollection<T>.Count => _itemFilePaths.Count;

    /// <inheritdoc cref="IList{T}.this" />
    public T this[int index]
    {
        get => GetItem(index);
        set => SetItem(index, value);
    }

    /// <inheritdoc/>
    public bool IsReadOnly => false;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemList{T}" /> class that is empty and has the default initial capacity.
    /// </summary>
    public FileSystemList() => _itemFilePaths = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemList{T}" /> class that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public FileSystemList(int capacity) => _itemFilePaths = new List<string>(capacity);

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemList{T}" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    public FileSystemList(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        _itemFilePaths = new List<string>();

        foreach (var item in collection)
            Add(item);
    }
    #endregion

    private readonly IList<string> _itemFilePaths;

    #region Public methods
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    /// <inheritdoc/>
    public void Add(T item) => Insert(_itemFilePaths.Count, item);

    /// <inheritdoc/>
    public void AddRange(IEnumerable<T> collection)
    {
        foreach (var item in collection)
            Add(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        foreach (var itemFilePath in _itemFilePaths)
            File.Delete(itemFilePath);

        _itemFilePaths.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(T item) => IndexOf(item) is not -1;

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (arrayIndex < 0 || arrayIndex > array.Length - 1)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Invalid array index.");

        var availableSpaceInArray = array.Length - arrayIndex;
        if (Count > availableSpaceInArray)
            throw new ArgumentException("There is not enough space in the array.");

        for (var i = arrayIndex; i < array.Length; i++)
            array[i] = GetItem(i);
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var itemIndex = IndexOf(item);
        if (itemIndex is -1)
            return false;

        RemoveAt(itemIndex);
        return true;
    }

    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        for (var i = 0; i < _itemFilePaths.Count; i++)
        {
            var otherItem = GetItem(i);
            if (EqualityComparer<T>.Default.Equals(item, otherItem))
                return i;
        }

        return -1;
    }

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        var itemFilePath = Path.GetTempFileName();
        _itemFilePaths.Insert(index, itemFilePath);

        var serializedItem = JsonSerializer.Serialize(item);
        File.WriteAllText(itemFilePath, serializedItem);
    }

    /// <inheritdoc cref="IList{T}"/>
    public void InsertRange(int index, IEnumerable<T> collection)
    {
        foreach (var item in collection)
            Insert(index++, item);
    }

    /// <inheritdoc cref="IExtendedList{T}"/>
    public void InsertRange(IEnumerable<T> collection, int index) => InsertRange(index, collection);

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        ValidateIndex(index);

        var itemFilePath = _itemFilePaths[index];
        _itemFilePaths.RemoveAt(index);
        File.Delete(itemFilePath);
    }

    /// <inheritdoc/>
    public void RemoveRange(int index, int count)
    {
        for (var i = 0; i < count; i++)
            RemoveAt(index);
    }

    /// <inheritdoc/>
    public void Move(int original, int destination)
    {
        var item = this[original];
        RemoveAt(original);
        Insert(destination, item);
    }

    #endregion

    ~FileSystemList() => Clear();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Private methods
    private T GetItem(int index)
    {
        ValidateIndex(index);

        var itemFilePath = _itemFilePaths[index];
        var serializedItem = File.ReadAllText(itemFilePath);
        return JsonSerializer.Deserialize<T>(serializedItem)!;
    }

    private void SetItem(int index, T newItem)
    {
        ValidateIndex(index);

        var itemFilePath = _itemFilePaths[index];
        var serializedItem = JsonSerializer.Serialize(newItem);
        File.WriteAllText(itemFilePath, serializedItem);
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index > _itemFilePaths.Count - 1)
            throw new ArgumentOutOfRangeException(nameof(index), index, "Invalid index.");
    }
    #endregion

    /// <summary>
    /// Enumerates the elements of a <see cref="FileSystemList{T}"/>.
    /// </summary>
    private struct Enumerator(FileSystemList<T> fileSystemList) : IEnumerator<T>
    {
        #region Properties
        /// <inheritdoc/>
        public T Current
        {
            get
            {
                if (_position > fileSystemList.Count - 1)
                    throw new InvalidOperationException();

                return fileSystemList[_position];
            }
        }

        /// <inheritdoc/>
        object? IEnumerator.Current => Current;
        #endregion

        private int _position = -1;

        #region Methods
        /// <inheritdoc/>
        public bool MoveNext()
        {
            _position++;
            return _position < fileSystemList.Count;
        }

        /// <inheritdoc/>
        public void Reset() => _position = -1;

        /// <summary>
        /// Releases all resources used by the <see cref="FileSystemList{T}.Enumerator"/>
        /// </summary>
        public void Dispose() { }
        #endregion
    }
}