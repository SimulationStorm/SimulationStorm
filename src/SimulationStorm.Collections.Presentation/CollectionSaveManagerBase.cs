using System.Linq;
using SimulationStorm.AppSaves;

namespace SimulationStorm.Collections.Presentation;

public abstract class CollectionSaveManagerBase<TItem, TSave>
(
    ICollectionManager<TItem> collectionManager
)
    : ServiceSaveManagerBase<TSave> where TSave : CollectionAndManagerSaveBase<TItem>, new()
{
    protected override TSave SaveServiceCore() => new()
    {
        IsSavingEnabled = collectionManager.IsSavingEnabled,
        SavingInterval = collectionManager.SavingInterval,
        StorageLocation = collectionManager.Collection.StorageLocation,
        Capacity = collectionManager.Collection.Capacity,
        Items = collectionManager.Collection.ToArray()
    };

    protected override void RestoreServiceSaveCore(TSave save)
    {
        collectionManager.IsSavingEnabled = save.IsSavingEnabled;
        collectionManager.SavingInterval = save.SavingInterval;
        collectionManager.Collection.StorageLocation = save.StorageLocation;
        collectionManager.Collection.Capacity = save.Capacity;
        
        collectionManager.Collection.Clear();
        collectionManager.Collection.AddRange(save.Items);
    }
}