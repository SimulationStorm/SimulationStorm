using System;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.AppStates.Presentation.Models;

public class AppStateModel(AppState appState) : IndexedObject
{
    public AppState AppState { get; } = appState;
    
    public DateTime DateAndTime => AppState.DateAndTime;

    public string Name
    {
        get => AppState.Name;
        set
        {
            AppState.Name = value;
            OnPropertyChanged();
        }
    }
}