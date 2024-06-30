using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.History.Presentation.Models;

namespace SimulationStorm.Simulation.History.Presentation.ViewModels;

public interface IHistoryViewModel : ICollectionManagerViewModel
{
    #region Properties
    ReadOnlyObservableCollection<HistoryRecordModel> RecordModels { get; }
    
    int CurrentSaveIndex { get; }
    #endregion

    #region Commands
    IAsyncRelayCommand GoToPreviousSaveCommand { get; }

    IAsyncRelayCommand GoToNextSaveCommand { get; }
    
    IAsyncRelayCommand GoToFirstSaveCommand { get; }

    IAsyncRelayCommand GoToLastSaveCommand { get; }

    IAsyncRelayCommand<int> GoToSaveCommand { get; }
    
    IAsyncRelayCommand<int> DeleteSaveCommand { get; }
    #endregion
}