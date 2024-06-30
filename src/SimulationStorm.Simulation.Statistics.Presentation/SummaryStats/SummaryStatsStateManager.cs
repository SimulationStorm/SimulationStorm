using System;
using System.Linq;
using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public class SummaryStatsStateManager<TSummary>(ISummaryStatsManager<TSummary> summaryStatsManager) : IServiceStateManager
{
    public Type StateType => typeof(SummaryStatsState<TSummary>);
    
    public object SaveServiceState() => new SummaryStatsState<TSummary>()
    {
        IsSavingEnabled = summaryStatsManager.IsSavingEnabled,
        SavingInterval = summaryStatsManager.SavingInterval,
        StorageLocation = summaryStatsManager.Collection.StorageLocation,
        Capacity = summaryStatsManager.Collection.Capacity,
        Items = summaryStatsManager.Collection.ToArray()
    };

    public void RestoreServiceState(object state)
    {
        var summaryStatsManagerState = (SummaryStatsState<TSummary>)state;
        
        summaryStatsManager.IsSavingEnabled = summaryStatsManagerState.IsSavingEnabled;
        summaryStatsManager.SavingInterval = summaryStatsManagerState.SavingInterval;
        summaryStatsManager.Collection.StorageLocation = summaryStatsManagerState.StorageLocation;
        summaryStatsManager.Collection.Capacity = summaryStatsManagerState.Capacity;
        
        summaryStatsManager.Collection.Clear();
        summaryStatsManager.Collection.AddRange(summaryStatsManagerState.Items);
    }
}