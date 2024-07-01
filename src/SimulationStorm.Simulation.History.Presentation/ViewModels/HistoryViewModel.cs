using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using DynamicData.Binding;
using SimulationStorm.Collections.Pointed;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.History.Presentation.ViewModels;

public partial class HistoryViewModel<TSave> :
    CollectionManagerViewModelBase<HistoryRecord<TSave>>,
    IHistoryViewModel
{
    #region Properties
    public ReadOnlyObservableCollection<HistoryRecordModel> RecordModels => _recordModels;

    public int CurrentSaveIndex => _historyManager.Collection.PointerPosition;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanGoToPreviousSave))]
    private async Task GoToPreviousSaveAsync()
    {
        await _historyManager.GoToPreviousSaveAsync();
        NotifyNavigationCommandsCanExecuteChanged();
    }
    private bool CanGoToPreviousSave() => _historyManager.CanGoToPreviousSave();

    
    [RelayCommand(CanExecute = nameof(CanGoToNextSave))]
    private async Task GoToNextSaveAsync()
    {
        await _historyManager.GoToNextSaveAsync();
        NotifyNavigationCommandsCanExecuteChanged();
    }
    private bool CanGoToNextSave() => _historyManager.CanGoToNextSave();
    
    
    [RelayCommand(CanExecute = nameof(CanGoToFirstSave))]
    private async Task GoToFirstSaveAsync()
    {
        await _historyManager.GoToFirstSaveAsync();
        NotifyNavigationCommandsCanExecuteChanged();
    }
    private bool CanGoToFirstSave() => _historyManager.CanGoToFirstSave();
    
    
    [RelayCommand(CanExecute = nameof(CanGoToLastSave))]
    private async Task GoToLastSaveAsync()
    {
        await _historyManager.GoToLastSaveAsync();
        NotifyNavigationCommandsCanExecuteChanged();
    }
    private bool CanGoToLastSave() => _historyManager.CanGoToLastSave();

    
    [RelayCommand(CanExecute = nameof(CanGoToSave))]
    private async Task GoToSaveAsync(int saveIndex)
    {
        await _historyManager.GoToSaveAsync(saveIndex);
        NotifyNavigationCommandsCanExecuteChanged();
    }
    private bool CanGoToSave(int saveIndex) => _historyManager.CanGoToSave(saveIndex);

    [RelayCommand]
    private Task DeleteSaveAsync(int saveIndex) => _historyManager.DeleteSaveAsync(saveIndex);

    private void NotifyNavigationCommandsCanExecuteChanged()
    {
        GoToPreviousSaveCommand.NotifyCanExecuteChanged();
        GoToNextSaveCommand.NotifyCanExecuteChanged();
        GoToFirstSaveCommand.NotifyCanExecuteChanged();
        GoToLastSaveCommand.NotifyCanExecuteChanged();
        GoToSaveCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Fields
    private readonly IHistoryManager<TSave> _historyManager;

    private ReadOnlyObservableCollection<HistoryRecordModel> _recordModels = null!;
    #endregion

    public HistoryViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        IImmediateUiThreadScheduler immediateUiThreadScheduler,
        IHistoryManager<TSave> historyManager,
        HistoryOptions options
    )
        : base(uiThreadScheduler, historyManager, options)
    {
        _historyManager = historyManager;

        _historyManager.Collection
            .IndexItemsAndBind<IUniversalCollection<HistoryRecord<TSave>>, HistoryRecord<TSave>, HistoryRecordModel>
            (
                record => new HistoryRecordModel(record.ExecutedCommand, record.SavingTime),
                out _recordModels,
                scheduler: immediateUiThreadScheduler
            )
            .DisposeWith(Disposables);

        _historyManager.Collection
            .ObserveCollectionChanges()
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => UpdateRecordModelPositions())
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler<CollectionPointerMovedEventArgs>, CollectionPointerMovedEventArgs>
            (
                h => _historyManager.Collection.PointerMoved += h,
                h => _historyManager.Collection.PointerMoved -= h
            )
            .ObserveOn(immediateUiThreadScheduler)
            .Subscribe(_ =>
            {
                UpdateRecordModelPositions();
                
                OnPropertyChanged(nameof(CurrentSaveIndex));
                NotifyNavigationCommandsCanExecuteChanged();
            })
            .DisposeWith(Disposables);
    }
    
    private void UpdateRecordModelPositions() => _recordModels.ForEach(UpdateRecordModelPosition);

    private void UpdateRecordModelPosition(HistoryRecordModel recordModel) =>
        recordModel.Position = GetRecordPositionByIndex(recordModel.Index);

    private HistoryRecordPosition GetRecordPositionByIndex(int recordIndex) =>
        recordIndex < CurrentSaveIndex
            ? HistoryRecordPosition.BehindPointer
            : recordIndex == CurrentSaveIndex
                ? HistoryRecordPosition.Pointed
                : HistoryRecordPosition.AheadOfPointer;
}