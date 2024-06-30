using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Indexing;

public abstract partial class IndexedObject : ObservableObject, IIndexedObject
{
    [NotifyPropertyChangedFor(nameof(OrdinalNumber))]
    [ObservableProperty]
    private int _index;

    public int OrdinalNumber => Index + 1;    
}