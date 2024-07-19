using System.Collections.ObjectModel;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Presentation;

public sealed class GenericCellularAutomationSettings
{
    public ObservableCollection<CellStateModel> CellStateModels { get; } = [];

    public GenericCellularAutomationSettings()
    {
    }
}