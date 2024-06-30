using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulationStorm.Utilities.UnitTests;

[TestClass]
public class IntervalActionExecutorTests
{
    [TestMethod]
    public void Is_Enabled_By_Default()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor();
        
        // Assert
        Assert.IsTrue(intervalActionExecutor.IsEnabled);
    }

    [TestMethod]
    public void Interval_Is_Zero_By_Default()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor();
        
        // Assert
        Assert.AreEqual(intervalActionExecutor.Interval, 0);
    }
    
    [TestMethod]
    public void Execution_Is_Needed_By_Default()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor();
        
        // Assert
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
    }
    
    [TestMethod]
    public void It_Is_Not_Possible_To_Set_Negative_Interval()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor();
        
        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => intervalActionExecutor.Interval = -1);
    }
    
    [TestMethod]
    public void Execution_Is_Not_Needed_When_Disabled()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            IsEnabled = false
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
    }
    
    [TestMethod]
    public void MoveNext_Works_As_Intended_When_Interval_Is_Zero()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 0
        };
        
        // Act & assert
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
    }
    
    [TestMethod]
    public void MoveNext_Works_As_Intended_When_Interval_Is_One()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 1
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
    }
    
    [TestMethod]
    public void MoveNext_Works_As_Intended_When_Interval_Is_Two()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 2
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
        
        intervalActionExecutor.MoveNext();
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        Assert.IsTrue(intervalActionExecutor.IsExecutionNeeded);
    }
    
    [TestMethod]
    public void GetIsExecutionNeededAndMoveNext_Works_As_Intended_When_Interval_Is_Zero()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 0
        };
        
        // Act & assert
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
    }
    
    [TestMethod]
    public void GetIsExecutionNeededAndMoveNext_Works_As_Intended_When_Interval_Is_One()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 1
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
    }
    
    [TestMethod]
    public void GetIsExecutionNeededAndMoveNext_Works_As_Intended_When_Interval_Is_Two()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 2
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsFalse(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
        Assert.IsTrue(intervalActionExecutor.GetIsExecutionNeededAndMoveNext());
    }
    
    [TestMethod]
    public void Disabling_Resets_Counter()
    {
        // Setup
        var intervalActionExecutor = new IntervalActionExecutor
        {
            Interval = 1
        };
        
        // Act & assert
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
        intervalActionExecutor.MoveNext();
        
        intervalActionExecutor.IsEnabled = false;
        intervalActionExecutor.IsEnabled = true;
        Assert.IsFalse(intervalActionExecutor.IsExecutionNeeded);
    }
}