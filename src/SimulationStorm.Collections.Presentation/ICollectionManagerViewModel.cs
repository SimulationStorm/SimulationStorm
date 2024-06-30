using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Primitives;

namespace SimulationStorm.Collections.Presentation;

public interface ICollectionManagerViewModel : INotifyPropertyChanged
{
    #region Properties
    bool IsSavingEnabled { get; set; }
    
    int SavingInterval { get; set; }
    
    CollectionStorageLocation EditingStorageLocation { get; set; }
    
    IEnumerable<CollectionStorageLocation> StorageLocations { get; }
    
    int EditingCapacity { get; set; }
    
    int Capacity { get; set; }
    
    bool AreThereRecords { get; }

    Range<int> SavingIntervalRange { get; }
    
    Range<int> CapacityRange { get; }
    #endregion

    #region Commands
    IAsyncRelayCommand ChangeStorageLocationCommand { get; }
    
    IAsyncRelayCommand ChangeCapacityCommand { get; }
    
    IAsyncRelayCommand ClearCommand { get; }
    
    IRelayCommand ResetSavingIntervalCommand { get; }
    
    IRelayCommand ResetEditingCapacityCommand { get; }
    #endregion
}