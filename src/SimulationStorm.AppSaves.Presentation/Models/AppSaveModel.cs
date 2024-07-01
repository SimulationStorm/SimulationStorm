using System;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.AppSaves.Presentation.Models;

public class AppSaveModel(AppSave appSave) : IndexedObject
{
    public AppSave AppSave { get; } = appSave;
    
    public DateTime DateAndTime => AppSave.DateAndTime;

    public string Name
    {
        get => AppSave.Name;
        set
        {
            AppSave.Name = value;
            OnPropertyChanged();
        }
    }
}