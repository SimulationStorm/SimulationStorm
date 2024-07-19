using CommunityToolkit.Mvvm.ComponentModel;
using SimulationStorm.Utilities.Indexing;

namespace GenericCellularAutomation.Presentation.Common;

public abstract partial class NamedIndexedObservableObject : IndexedObject, INamedObject
{
    [ObservableProperty] private string _name = string.Empty;
}