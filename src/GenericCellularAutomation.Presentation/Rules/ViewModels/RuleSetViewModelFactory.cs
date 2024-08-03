using System.Collections.ObjectModel;
using GenericCellularAutomation.Presentation.CellStates;
using SimulationStorm.Localization.Presentation;

namespace GenericCellularAutomation.Presentation.Rules.ViewModels;

public sealed class RuleSetViewModelFactory
(
    ILocalizationManager localizationManager,
    RulesOptions rulesOptions
)
    : IRuleSetViewModelFactory
{
    public RuleSetViewModel CreateRuleSetViewModel(ReadOnlyObservableCollection<CellStateModel> cellStateModels) =>
        new(localizationManager, rulesOptions, cellStateModels);
}