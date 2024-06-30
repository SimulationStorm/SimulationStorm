using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Collections.Universal;

namespace SimulationStorm.Collections.UnitTests;

// Todo: complete unit testing of universal collection.

[TestClass]
public class UniversalCollectionTests
{
    private static readonly IListFactory ListFactory = new ListFactory();

    #region IList interface testing
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Addition_works_as_intended(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);

        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        // Assert
        for (var i = 0; i < itemCount; i++)
            Assert.AreEqual(i, universalCollection[i]);
    }
    
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Removing_works_as_intended(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);

        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        for (var i = 0; i < itemCount; i++)
            universalCollection.RemoveAt(0);
        
        Assert.AreEqual(0, universalCollection.Count);
    }
    
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Clearing_works_as_intended(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);

        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        universalCollection.Clear();
        
        Assert.AreEqual(0, universalCollection.Count);
    }
    #endregion

    #region IRoundCollection interface testing
    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    public void Can_not_have_negative_or_zero_capacity(int capacity)
    {
        // Setup & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => CreateUniversalCollection<object>(capacity));
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Capacity_works_as_intended(int capacity)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(capacity);

        // Act
        for (var i = 0; i < capacity + 1; i++)
            universalCollection.Add(i);
        
        // Assert
        for (var i = 0; i < capacity; i++)
            Assert.AreEqual(i + 1, universalCollection[i]);
    }
    #endregion

    #region IPointedCollection interface testing
    [TestMethod]
    public void Default_pointer_position_is_minus_one()
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(1);
        
        // Assert
        Assert.AreEqual(-1, universalCollection.PointerPosition);
    }
    
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Pointer_could_not_be_moved_beyond_collection(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);

        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => universalCollection.MovePointer(itemCount));
    }

    [DataTestMethod]
    [DataRow(-2)]
    [DataRow(-3)]
    [DataRow(-4)]
    public void Pointer_could_not_be_moved_to_position_less_than_default(int position)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(1);
        
        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => universalCollection.MovePointer(position));
    }
    
    [TestMethod]
    public void Pointer_could_be_moved_to_default_position()
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(1);

        // Act
        universalCollection.Add(0);
        universalCollection.MovePointer(-1);
        
        // Assert
        Assert.AreEqual(-1, universalCollection.PointerPosition);
    }
    
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Pointer_moves_as_items_are_added(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);
    
        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        // Assert
        Assert.AreEqual(itemCount - 1, universalCollection.PointerPosition);
    }

    [DataTestMethod]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public void Items_after_pointer_are_removed_when_pointer_was_manually_moved_and_items_are_added(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);
    
        // Act
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);
        
        universalCollection.MovePointer(0);
        universalCollection.Add(0);
        
        // Assert
        Assert.AreEqual(2, universalCollection.Count);
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public void Pointer_automatically_moves_to_valid_position_when_items_are_removed(int itemCount)
    {
        // Setup
        var universalCollection = CreateUniversalCollection<int>(itemCount);
    
        // Act & assert
        for (var i = 0; i < itemCount; i++)
            universalCollection.Add(i);

        for (var i = 0; i < itemCount; i++)
        {
            universalCollection.RemoveAt(0);
            Assert.AreEqual(universalCollection.PointerPosition, universalCollection.Count - 1);
        }
    }
    #endregion

    private static IUniversalCollection<T> CreateUniversalCollection<T>(
        int capacity, CollectionStorageLocation storageLocation = CollectionStorageLocation.Memory) =>
            new UniversalCollection<T>(ListFactory, storageLocation, capacity);
}