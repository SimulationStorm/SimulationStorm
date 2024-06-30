using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistoryManager<TSave> : CollectionManagerBase<HistoryRecord<TSave>>, IHistoryManager<TSave>
{
    public event EventHandler<SimulationCommandExecutedEventArgs>? SimulationCommandExecutedEventHandled;
    
    private readonly ISaveableSimulationManager<TSave> _simulationManager;

    public HistoryManager
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ISaveableSimulationManager<TSave> simulationManager,
        HistoryOptions options
    )
        : base(universalCollectionFactory, intervalActionExecutor, options)
    {
        _simulationManager = simulationManager;

        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SimulationCommandExecutedEventArgs>, SimulationCommandExecutedEventArgs>
                (
                    h => _simulationManager.CommandExecuted += h,
                    h => _simulationManager.CommandExecuted -= h
                )
                .Subscribe(e => _ = HandleSimulationCommandExecutedAsync(e.EventArgs).ConfigureAwait(false))
                .DisposeWith(disposables);
        });

        // CreateInitialRecordIfSavingIsEnabledAsync();
    }

    #region Public methods
    public bool CanGoToPreviousSave() => CanGoToSave(Collection.PointerPosition - 1);
    public Task GoToPreviousSaveAsync() => GoToSaveAsync(Collection.PointerPosition - 1);

    public bool CanGoToNextSave() => CanGoToSave(Collection.PointerPosition + 1);
    public Task GoToNextSaveAsync() => GoToSaveAsync(Collection.PointerPosition + 1);

    public bool CanGoToFirstSave() => CanGoToSave(0);
    public Task GoToFirstSaveAsync() => GoToSaveAsync(0);
    
    public bool CanGoToLastSave() => CanGoToSave(Collection.Count - 1);
    public Task GoToLastSaveAsync() => GoToSaveAsync(Collection.Count - 1);

    public bool CanGoToSave(int saveIndex) =>  IsValidSaveIndex(saveIndex) && saveIndex != Collection.PointerPosition;
    public async Task GoToSaveAsync(int saveIndex)
    {
        // ThrowIfSaveIndexIsInvalid(saveIndex); // Commented for a while
        if (!CanGoToSave(saveIndex))
            return;
        
        var record = Collection[saveIndex];
        var save = record.Save;
        
        await _simulationManager
            .RestoreStateAsync(save)
            .ConfigureAwait(false);
        
        Collection.MovePointer(saveIndex);
    }

    public async Task DeleteSaveAsync(int saveIndex)
    {
        ThrowIfSaveIndexIsInvalid(saveIndex);
        await Task.Run(() => Collection.RemoveAt(saveIndex));
    }
    #endregion

    // protected override void OnIsSavingEnabledChanged()
    // {
        // base.OnIsSavingEnabledChanged();

        // CreateInitialRecordIfSavingIsEnabledAsync();
    // }

    #region Private methods
    private void ThrowIfSaveIndexIsInvalid(int saveIndex)
    {
        if (!IsValidSaveIndex(saveIndex))
            throw new ArgumentOutOfRangeException(nameof(saveIndex), saveIndex, "Must be a valid save index.");
    }

    private bool IsValidSaveIndex(int saveIndex) => saveIndex >= 0 && saveIndex < Collection.Count;

    // private Task CreateInitialRecordIfSavingIsEnabledAsync() => IsSavingEnabled
    //         ? SaveSimulationAndAddRecordToCollectionAsync(new NullCommand(), TimeSpan.Zero).ThrowWhenFaulted()
    //         : Task.CompletedTask;

    private async Task HandleSimulationCommandExecutedAsync(SimulationCommandExecutedEventArgs e)
    {
        if (e.Command is not RestoreStateCommand && IntervalActionExecutor.GetIsExecutionNeededAndMoveNext())
            await SaveSimulationAndAddRecordToCollectionAsync(e.Command).ConfigureAwait(false);
        
        e.Synchronizer.Signal();
        
        SimulationCommandExecutedEventHandled?.Invoke(this, e);
    }

    private async Task SaveSimulationAndAddRecordToCollectionAsync(SimulationCommand command)
    {
        var benchmarkResult = await _simulationManager
            .SaveAndMeasureAsync()
            .ConfigureAwait(false);
        
        var save = benchmarkResult.FunctionResult!;
        var timeElapsedOnSaving = benchmarkResult.ElapsedTime;
        
        var record = new HistoryRecord<TSave>(command, save, timeElapsedOnSaving);
        Collection.Add(record);
    }
    #endregion
}