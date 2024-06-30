using System.ComponentModel;
using SimulationStorm.Collections.Universal;

namespace SimulationStorm.Collections.Presentation;

public interface ICollectionManager<T> : INotifyPropertyChanged
{
    IUniversalCollection<T> Collection { get; }
    
    bool IsSavingEnabled { get; set; }
    
    int SavingInterval { get; set; }
}