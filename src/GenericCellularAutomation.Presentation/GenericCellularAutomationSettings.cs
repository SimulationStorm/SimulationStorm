using System.Collections.ObjectModel;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Rules.Models;

namespace GenericCellularAutomation.Presentation;

public sealed class GenericCellularAutomationSettings
{
    public ObservableCollection<CellStateModel> CellStateModels { get; } = [];

    public ObservableCollection<RuleSetModel> RuleSetModels { get; } = [];
}