using System.Linq;
using SimulationStorm.AppStates;

namespace SimulationStorm.Collections.Presentation;

public abstract class CollectionStateManagerBase<T, TState>
(
    ICollectionManager<T> collectionManager
)
    : ServiceStateManagerBase<TState> where TState : CollectionAndManagerStateBase<T>, new()
{
    protected override TState SaveServiceStateImpl() => new()
    {
        IsSavingEnabled = collectionManager.IsSavingEnabled,
        SavingInterval = collectionManager.SavingInterval,
        StorageLocation = collectionManager.Collection.StorageLocation,
        Capacity = collectionManager.Collection.Capacity,
        Items = collectionManager.Collection.ToArray()
    };

    protected override void RestoreServiceStateImpl(TState state)
    {
        collectionManager.IsSavingEnabled = state.IsSavingEnabled;
        collectionManager.SavingInterval = state.SavingInterval;
        collectionManager.Collection.StorageLocation = state.StorageLocation;
        collectionManager.Collection.Capacity = state.Capacity;
        
        collectionManager.Collection.Clear();
        collectionManager.Collection.AddRange(state.Items);
    }
}