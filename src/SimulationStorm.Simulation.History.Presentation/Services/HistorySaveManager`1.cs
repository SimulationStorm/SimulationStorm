using System;
using System.Linq;
using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistorySaveManager<TSave>(IHistoryManager<TSave> historyManager) : IServiceSaveManager
{
    public Type SaveType => typeof(HistorySave<TSave>);
    
    public object SaveService() => new HistorySave<TSave>
    {
        IsSavingEnabled = historyManager.IsSavingEnabled,
        SavingInterval = historyManager.SavingInterval,
        StorageLocation = historyManager.Collection.StorageLocation,
        Capacity = historyManager.Collection.Capacity,
        Items = historyManager.Collection.ToArray()
    };

    public void RestoreServiceSave(object save)
    {
        var historyState = (HistorySave<TSave>)save;
        
        historyManager.IsSavingEnabled = historyState.IsSavingEnabled;
        historyManager.SavingInterval = historyState.SavingInterval;
        historyManager.Collection.StorageLocation = historyState.StorageLocation;
        historyManager.Collection.Capacity = historyState.Capacity;
        
        historyManager.Collection.Clear();
        historyManager.Collection.AddRange(historyState.Items);
    }
}