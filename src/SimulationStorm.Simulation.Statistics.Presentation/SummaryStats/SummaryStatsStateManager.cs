using System;
using System.Linq;
using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public class SummaryStatsSaveManager<TSummary>(ISummaryStatsManager<TSummary> summaryStatsManager) : IServiceSaveManager
{
    public Type SaveType => typeof(SummaryStatsSave<TSummary>);
    
    public object SaveService() => new SummaryStatsSave<TSummary>()
    {
        IsSavingEnabled = summaryStatsManager.IsSavingEnabled,
        SavingInterval = summaryStatsManager.SavingInterval,
        StorageLocation = summaryStatsManager.Collection.StorageLocation,
        Capacity = summaryStatsManager.Collection.Capacity,
        Items = summaryStatsManager.Collection.ToArray()
    };

    public void RestoreServiceSave(object save)
    {
        var summaryStatsManagerState = (SummaryStatsSave<TSummary>)save;
        
        summaryStatsManager.IsSavingEnabled = summaryStatsManagerState.IsSavingEnabled;
        summaryStatsManager.SavingInterval = summaryStatsManagerState.SavingInterval;
        summaryStatsManager.Collection.StorageLocation = summaryStatsManagerState.StorageLocation;
        summaryStatsManager.Collection.Capacity = summaryStatsManagerState.Capacity;
        
        summaryStatsManager.Collection.Clear();
        summaryStatsManager.Collection.AddRange(summaryStatsManagerState.Items);
    }
}