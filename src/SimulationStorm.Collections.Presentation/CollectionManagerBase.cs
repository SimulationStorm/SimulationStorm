using SimulationStorm.Collections.Universal;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Collections.Presentation;

public abstract class CollectionManagerBase<T> : DisposableObservableObject, ICollectionManager<T>
{
    public IUniversalCollection<T> Collection { get; }
    
    #region Properties
    public bool IsSavingEnabled
    {
        get => IntervalActionExecutor.IsEnabled;
        set
        {
            var wasPropertySet = SetProperty(
                IntervalActionExecutor.IsEnabled, value, IntervalActionExecutor, (o, v) => o.IsEnabled = v);
            
            if (wasPropertySet)
                Collection.Clear();
        }
    }

    public int SavingInterval
    {
        get => IntervalActionExecutor.Interval;
        set => SetProperty(IntervalActionExecutor.Interval, value, IntervalActionExecutor, (o, v) => o.Interval = v);
    }
    #endregion
    
    protected IIntervalActionExecutor IntervalActionExecutor { get; }

    protected CollectionManagerBase
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ICollectionManagerOptions options)
    {
        Collection = universalCollectionFactory.Create<T>(options.StorageLocation, options.Capacity);
        IntervalActionExecutor = intervalActionExecutor;

        IntervalActionExecutor.IsEnabled = options.IsSavingEnabled;
        IntervalActionExecutor.Interval = options.SavingInterval;
    }
}