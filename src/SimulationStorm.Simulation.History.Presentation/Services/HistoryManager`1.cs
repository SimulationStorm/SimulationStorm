using System;
using System.Threading.Tasks;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistoryManager<TSave>
(
    IUniversalCollectionFactory universalCollectionFactory,
    IIntervalActionExecutor intervalActionExecutor,
    ISaveableSimulationManager<TSave> simulationManager,
    HistoryOptions options
)
    : CollectionManagerBase<HistoryRecord<TSave>>(universalCollectionFactory, intervalActionExecutor, options),
        IHistoryManager<TSave>
{
    public event EventHandler<SimulationCommandCompletedEventArgs>? SimulationCommandCompletedEventHandled;

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
        
        await simulationManager
            .RestoreStateAsync(save)
            .ConfigureAwait(false);
        
        Collection.MovePointer(saveIndex);
    }

    public async Task DeleteSaveAsync(int saveIndex)
    {
        ThrowIfSaveIndexIsInvalid(saveIndex);
        await Task.Run(() => Collection.RemoveAt(saveIndex));
    }

    public async Task HandleSimulationCommandCompletedAsync(SimulationCommandCompletedEventArgs e)
    {
        if (!IsSavingNeeded(e.Command))
            return;
        
        await SaveSimulationAndAddRecordToCollectionAsync(e.Command)
            .ConfigureAwait(false);
        
        SimulationCommandCompletedEventHandled?.Invoke(this, e);
    }
    #endregion

    #region Private methods
    private void ThrowIfSaveIndexIsInvalid(int saveIndex)
    {
        if (!IsValidSaveIndex(saveIndex))
            throw new ArgumentOutOfRangeException(nameof(saveIndex), saveIndex, "Must be a valid save index.");
    }

    private bool IsValidSaveIndex(int saveIndex) => saveIndex >= 0 && saveIndex < Collection.Count;

    private bool IsSavingNeeded(SimulationCommand command) =>
        !IsDisposingOrDisposed
        && command is not RestoreStateCommand
        && IntervalActionExecutor.GetIsExecutionNeededAndMoveNext();
    
    private async Task SaveSimulationAndAddRecordToCollectionAsync(SimulationCommand command)
    {
        var benchmarkResult = await simulationManager
            .SaveAndMeasureAsync()
            .ConfigureAwait(false);
        
        var save = benchmarkResult.FunctionResult!;
        var timeElapsedOnSaving = benchmarkResult.ElapsedTime;
        
        var record = new HistoryRecord<TSave>(command, save, timeElapsedOnSaving);
        Collection.Add(record);
    }
    #endregion
}