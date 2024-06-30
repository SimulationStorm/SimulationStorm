using System;
using System.Linq;
using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistoryStateManager<TState>(IHistoryManager<TState> historyManager) : IServiceStateManager
{
    public Type StateType => typeof(HistoryState<TState>);
    
    public object SaveServiceState() => new HistoryState<TState>
    {
        IsSavingEnabled = historyManager.IsSavingEnabled,
        SavingInterval = historyManager.SavingInterval,
        StorageLocation = historyManager.Collection.StorageLocation,
        Capacity = historyManager.Collection.Capacity,
        Items = historyManager.Collection.ToArray()
    };

    public void RestoreServiceState(object state)
    {
        var historyState = (HistoryState<TState>)state;
        
        historyManager.IsSavingEnabled = historyState.IsSavingEnabled;
        historyManager.SavingInterval = historyState.SavingInterval;
        historyManager.Collection.StorageLocation = historyState.StorageLocation;
        historyManager.Collection.Capacity = historyState.Capacity;
        
        historyManager.Collection.Clear();
        historyManager.Collection.AddRange(historyState.Items);
    }
}