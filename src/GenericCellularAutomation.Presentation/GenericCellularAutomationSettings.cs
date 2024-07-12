using System.Collections.ObjectModel;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Presentation;

public class GenericCellularAutomationSettings
{
    public ReadOnlyObservableCollection<CellStateModel> CellStateModels { get; }

    private readonly ObservableCollection<CellStateModel> _cellStateModels = [];
    
    public GenericCellularAutomationSettings()
    {
        CellStateModels = new ReadOnlyObservableCollection<CellStateModel>(_cellStateModels);
    }
}