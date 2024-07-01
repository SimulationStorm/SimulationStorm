using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DotNext.Threading;
using DynamicData;
using DynamicData.Binding;
using SimulationStorm.Collections.Pointed;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public class HistoryAwareSummaryStatsManager<TSummary, TSave> :
    CollectionManagerBase<SummaryRecord<TSummary>>,
    ISummaryStatsManager<TSummary>
{
    #region Fields
    private readonly ISummarizableSimulationManager<TSummary> _simulationManager;

    private readonly IHistoryManager<TSave> _historyManager;

    private readonly AsyncAutoResetEvent _commandCompletedEventSynchronizer = new(false);
    
    private readonly IDictionary<SummaryRecord<TSummary>, HistoryRecord<TSave>?>
        _historyRecordsBySummaryRecord = new Dictionary<SummaryRecord<TSummary>, HistoryRecord<TSave>?>();

    // private readonly IList<(int SummaryRecordIndex, SummaryRecord<TSummary> SummaryRecord,
    //     int HistoryRecordIndex, HistoryRecord<TSaving> historyRecord)> _test =
    //         new List<(int SummaryRecordIndex, SummaryRecord<TSummary> SummaryRecord,
    //             int HistoryRecordIndex, HistoryRecord<TSaving> historyRecord)>();
    
    private bool _wereSummaryRecordsHidden;
    #endregion

    public HistoryAwareSummaryStatsManager
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ISummarizableSimulationManager<TSummary> simulationManager,
        IHistoryManager<TSave> historyManager,
        ISummaryStatsOptions options
    )
        : base(universalCollectionFactory, intervalActionExecutor, options)
    {
        _simulationManager = simulationManager;
        _historyManager = historyManager;
        
        // After history manager completing its work, make new summary record if needed
        Observable
            .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
            (
                h => _historyManager.SimulationCommandCompletedEventHandled += h,
                h => _historyManager.SimulationCommandCompletedEventHandled += h
            )
            .Select(e => e.EventArgs)
            .Subscribe(e => _commandCompletedEventSynchronizer.Set())
            .DisposeWith(Disposables);

        Observable
            .FromEventPattern<EventHandler<CollectionPointerMovedEventArgs>, CollectionPointerMovedEventArgs>
            (
                h => _historyManager.Collection.PointerMoved += h,
                h => _historyManager.Collection.PointerMoved -= h
            )
            .Select(e => e.EventArgs)
            .Subscribe(e =>
            {
                var start = e.MovementDirection is PointerMovementDirection.Forward
                    ? e.OldPosition + 1
                    : e.NewPosition + 1;
                var count = e.PositionDelta;
                var affectedHistoryRecords = _historyManager.Collection
                    .Skip(start)
                    .Take(count)
                    .ToList();
                
                var summaryRecords = _historyRecordsBySummaryRecord
                    .Where(x => x.Value is not null && affectedHistoryRecords.Contains(x.Value))
                    .Select(x => x.Key)
                    .ToList();
                
                if (e.MovementDirection is PointerMovementDirection.Backward)
                {
                    _wereSummaryRecordsHidden = true;
                    Collection.RemoveMany(summaryRecords);
                    _wereSummaryRecordsHidden = false;
                }
                else
                {
                    Collection.Add(summaryRecords);
                }
            })
            .DisposeWith(Disposables);

        // If history record was removed, remove associated summary records
        _historyManager.Collection
            .ToObservableChangeSet<IUniversalCollection<HistoryRecord<TSave>>, HistoryRecord<TSave>>()
            .OnItemRemoved(historyRecord =>
            {
                // Here, we should remove records
                var summaryRecords = _historyRecordsBySummaryRecord
                    .Where(x => x.Value == historyRecord)
                    .Select(x => x.Key)
                    .ToList();
                
                Collection.RemoveMany(summaryRecords);
            })
            .Subscribe()
            .DisposeWith(Disposables);
        
        // If summary record was removed, remove association if association exists
        Collection
            .ToObservableChangeSet<IUniversalCollection<SummaryRecord<TSummary>>, SummaryRecord<TSummary>>()
            .OnItemRemoved(summaryRecord =>
            {
                if (_wereSummaryRecordsHidden)
                    return;
                
                _historyRecordsBySummaryRecord.Remove(summaryRecord);
            })
            .Subscribe()
            .DisposeWith(Disposables);
    }

    public async Task HandleSimulationCommandCompletedAsync(SimulationCommandCompletedEventArgs e)
    {
        if (!IsSummarizingNeeded(e.Command))
            return;
        
        await _commandCompletedEventSynchronizer
            .WaitAsync()
            .ConfigureAwait(false);
        
        await SummarizeSimulationAndAddToCollection(e.Command)
            .ConfigureAwait(false);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _commandCompletedEventSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
        
        await base.DisposeAsyncCore()
            .ConfigureAwait(false);
    }

    #region Private methods
    private bool IsSummarizingNeeded(SimulationCommand command) =>
        !IsDisposingOrDisposed
        && command.ChangesWorld
        && command is not RestoreStateCommand { IsRestoringFromAppSave: true }
        && IntervalActionExecutor.GetIsExecutionNeededAndMoveNext();

    private async Task SummarizeSimulationAndAddToCollection(SimulationCommand command)
    {
        var benchmarkResult = await _simulationManager
            .SummarizeAndMeasureAsync()
            .ConfigureAwait(false);
        
        var summary = benchmarkResult.FunctionResult!;
        var elapsedTime = benchmarkResult.ElapsedTime;
        
        var record = new SummaryRecord<TSummary>(command, summary, elapsedTime);
        Collection.Add(record);

        _historyRecordsBySummaryRecord[record] = null;

        if (_historyManager.Collection.Count > 0)
        {
            var lastHistoryRecord = _historyManager.Collection[^1];

            var summaryRecordsNotAssociatedWithHistoryRecords = _historyRecordsBySummaryRecord
                .Where(x => x.Value is null)
                .Select(x => x.Key);

            foreach (var summaryRecord in summaryRecordsNotAssociatedWithHistoryRecords)
                _historyRecordsBySummaryRecord[summaryRecord] = lastHistoryRecord;
        }
    }
    #endregion
}