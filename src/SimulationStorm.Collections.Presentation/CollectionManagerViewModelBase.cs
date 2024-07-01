using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Primitives;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Collections.Presentation;

public abstract partial class CollectionManagerViewModelBase<T> : DisposableObservableObject, ICollectionManagerViewModel
{
    #region Properties
    public bool IsSavingEnabled
    {
        get => _collectionManager.IsSavingEnabled;
        set => _collectionManager.IsSavingEnabled = value;
    }
    
    public int SavingInterval
    {
        get => _collectionManager.SavingInterval;
        set => _collectionManager.SavingInterval = value;
    }

    [NotifyCanExecuteChangedFor(nameof(ChangeStorageLocationCommand))]
    [ObservableProperty]
    private CollectionStorageLocation _editingStorageLocation;

    public IEnumerable<CollectionStorageLocation> StorageLocations { get; } = Enum.GetValues<CollectionStorageLocation>();

    public int Capacity
    {
        get => _collectionManager.Collection.Capacity;
        set => _collectionManager.Collection.Capacity = value;
    }
    
    [NotifyCanExecuteChangedFor(nameof(ChangeCapacityCommand))]
    [ObservableProperty]
    private int _editingCapacity;

    [ObservableProperty] private bool _areThereRecords;

    public Range<int> CapacityRange { get; }

    public Range<int> SavingIntervalRange { get; }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanChangeStorageLocation))]
    private async Task ChangeStorageLocationAsync()
    {
        await Task.Run(() => _collectionManager.Collection.StorageLocation = EditingStorageLocation);
        ChangeStorageLocationCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeStorageLocation() => EditingStorageLocation != _collectionManager.Collection.StorageLocation;


    [RelayCommand(CanExecute = nameof(CanChangeCapacity))]
    private async Task ChangeCapacityAsync()
    {
        await Task.Run(() => _collectionManager.Collection.Capacity = EditingCapacity);
        ChangeCapacityCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeCapacity() => EditingCapacity != _collectionManager.Collection.Capacity;
    
    
    [RelayCommand(CanExecute = nameof(CanResetSavingInterval))]
    private void ResetSavingInterval() => SavingInterval = _options.SavingInterval;
    private bool CanResetSavingInterval() => SavingInterval != _options.SavingInterval;

    
    [RelayCommand(CanExecute = nameof(CanResetEditingCapacity))]
    private void ResetEditingCapacity() => EditingCapacity = _options.Capacity;
    private bool CanResetEditingCapacity() => EditingCapacity != _options.Capacity;

    [RelayCommand(CanExecute = nameof(CanClear))]
    private async Task Clear()
    {
        await Task.Run(() => _collectionManager.Collection.Clear());
        ClearCommand.NotifyCanExecuteChanged();
    }
    private bool CanClear() => _collectionManager.Collection.Count is not 0;
    #endregion

    #region Fields
    private readonly ICollectionManager<T> _collectionManager;

    private readonly ICollectionManagerOptions _options;
    #endregion

    protected CollectionManagerViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        ICollectionManager<T> collectionManager,
        ICollectionManagerOptions options)
    {
        _collectionManager = collectionManager;
        _options = options;
        _editingCapacity = collectionManager.Collection.Capacity;
        CapacityRange = options.CapacityRange;
        SavingIntervalRange = options.SavingIntervalRange;
        
        _collectionManager
            .WhenValueChanged(x => x.IsSavingEnabled, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => OnPropertyChanged(nameof(IsSavingEnabled)))
            .DisposeWith(Disposables);
        
        _collectionManager
            .WhenValueChanged(x => x.SavingInterval, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ =>
            {
                OnPropertyChanged(nameof(SavingInterval));
                ResetSavingIntervalCommand.NotifyCanExecuteChanged();
            })
            .DisposeWith(Disposables);

        _collectionManager.Collection
            .WhenValueChanged(x => x.StorageLocation, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(newStorageLocation => EditingStorageLocation = newStorageLocation)
            .DisposeWith(Disposables);
        
        _collectionManager.Collection
            .WhenValueChanged(x => x.Capacity, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(newCapacity =>
            {
                OnPropertyChanged(nameof(Capacity));
                EditingCapacity = newCapacity;
            })
            .DisposeWith(Disposables);
        
        collectionManager.Collection
            .WhenValueChanged(x => x.Count)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ =>
            {
                AreThereRecords = collectionManager.Collection.Count is not 0;
                ClearCommand.NotifyCanExecuteChanged();
            })
            .DisposeWith(Disposables);
    }
}