using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public class SummaryStatsManager<TSummary> : CollectionManagerBase<SummaryRecord<TSummary>>, ISummaryStatsManager<TSummary>
{
    private readonly ISummarizableSimulationManager<TSummary> _simulationManager;

    public SummaryStatsManager
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ISummarizableSimulationManager<TSummary> simulationManager,
        ISummaryStatsOptions options
    )
        : base(universalCollectionFactory, intervalActionExecutor, options)
    {
        _simulationManager = simulationManager;
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => _simulationManager.CommandCompleted += h,
                    h => _simulationManager.CommandCompleted += h
                )
                .Select(e => e.EventArgs)
                .Subscribe(e => HandleSimulationCommandExecutedAsync(e).ConfigureAwait(false))
                .DisposeWith(disposables);
        });

        // CreateInitialRecordIfSavingIsEnabledAsync();
    }
    
    // protected override void OnIsSavingEnabledChanged()
    // {
    //     base.OnIsSavingEnabledChanged();

        // CreateInitialRecordIfSavingIsEnabledAsync();
    // }

    #region Private methods
    // private Task CreateInitialRecordIfSavingIsEnabledAsync() => IsSavingEnabled
    //     ? SummarizeSimulationAndAddToCollection(new NullCommand()).ThrowWhenFaulted()
    //     : Task.CompletedTask;
    
    private async Task HandleSimulationCommandExecutedAsync(SimulationCommandCompletedEventArgs e)
    {
        var isSummarizingNeeded =
            e.Command is not RestoreStateCommand { IsRestoringFromAppState: true }
            && e.Command.ChangesWorld
            && IntervalActionExecutor.GetIsExecutionNeededAndMoveNext();
        
        if (isSummarizingNeeded)
            await SummarizeSimulationAndAddToCollection(e.Command).ConfigureAwait(false);

        e.Synchronizer.Signal();
    }

    private async Task SummarizeSimulationAndAddToCollection(SimulationCommand command)
    {
        var benchmarkResult = await _simulationManager
            .SummarizeAndMeasureAsync()
            .ConfigureAwait(false);
        
        var summary = benchmarkResult.FunctionResult!;
        var elapsedTime = benchmarkResult.ElapsedTime;
        
        var records = new SummaryRecord<TSummary>(command, summary, elapsedTime);
        Collection.Add(records);
    }
    #endregion
}