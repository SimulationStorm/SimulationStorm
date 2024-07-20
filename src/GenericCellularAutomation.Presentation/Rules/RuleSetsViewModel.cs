using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericCellularAutomation.Presentation.Management;
using GenericCellularAutomation.Presentation.Rules.Models;
using SimulationStorm.Localization.Presentation;

namespace GenericCellularAutomation.Presentation.Rules;

public sealed partial class RuleSetsViewModel
(
    GenericCellularAutomationManager gcaManager,
    GenericCellularAutomationSettings gcaSettings,
    ILocalizationManager localizationManager,
    RulesAndRuleSetsOptions options
)
    : ObservableObject
{
    #region Properties
    [ObservableProperty] private int _repetitionCount;

    public ObservableCollection<RuleSetModel> RuleSetModels { get; } = [];
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddRuleSet))]
    private void AddRuleSet()
    {
        var ruleSetModel = new RuleSetModel
        {
            Index = RuleSetModels.Count,
            Name = GetNewRuleSetName(),
            RepetitionCount = options.RuleSetRepetitionCountRange.Minimum,
            RuleModels = {  }
        };
        
        RuleSetModels.Add(ruleSetModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddRuleSet() => RuleSetModels.Count < options.MaxRuleSetCount;

    [RelayCommand(CanExecute = nameof(CanRemoveRuleSet))]
    private void RemoveRuleSet(RuleSetModel ruleSetModel)
    {
        RuleSetModels.Remove(ruleSetModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveRuleSet() => RuleSetModels.Count > 1;

    [RelayCommand(CanExecute = nameof(CanApplyChanges))]
    private async Task ApplyChangesAsync()
    {
        
    }

    private bool CanApplyChanges()
    {
        
    }

    private void NotifyCommandsCanExecuteChanged()
    {
        AddRuleSetCommand.NotifyCanExecuteChanged();
        RemoveRuleSetCommand.NotifyCanExecuteChanged();
        ApplyChangesCommand.NotifyCanExecuteChanged();
    }
    #endregion

    private string GetNewRuleSetName() =>
        localizationManager.GetLocalizedString("Simulation.Gca.NewRuleSet");
}