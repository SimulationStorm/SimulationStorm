using System.Collections.ObjectModel;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Presentation.Rules.ViewModels;

public interface IRuleSetViewModelFactory
{
    RuleSetViewModel CreateRuleSetViewModel(ReadOnlyObservableCollection<CellStateModel> cellStateModels);
}