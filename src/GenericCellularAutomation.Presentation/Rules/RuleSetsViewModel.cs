using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericCellularAutomation.Presentation.Management;
using GenericCellularAutomation.Presentation.Rules.Models;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules;

public sealed partial class RuleSetsViewModel
(
    GenericCellularAutomationManager gcaManager,
    GenericCellularAutomationSettings gcaSettings,
    GenericCellularAutomationOptions options
)
    : ObservableObject
{
    #region Properties
    [ObservableProperty] private int _repetitionCount;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddRuleSet))]
    private async Task AddRuleSetAsync()
    {
        var ruleSetModel = new RuleSetModel
        {
            
        };
        
        // Todo: which should be created first?
        var ruleSet = new RuleSet();
        
        await gcaManager
            .ChangeRuleSetCollectionAsync(gcaManager.RuleSetCollection
                .WithRuleSet(ruleSet))
            .ConfigureAwait(false); 
        
        gcaSettings.RuleSetModels.Add(ruleSetModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddRuleSet() => gcaSettings.RuleSetModels.Count < options.MaxRuleSetCount;

    [RelayCommand(CanExecute = nameof(CanRemoveRuleSet))]
    private async Task RemoveRuleSetAsync(RuleSetModel ruleSetModel)
    {
        var ruleSetIndex = gcaSettings.RuleSetModels.IndexOf(ruleSetModel);
        // Todo: consider using IReadOnlyList instead of IEnumerable everywhere...
        var ruleSet = gcaManager.RuleSetCollection.RuleSets.ElementAt(ruleSetIndex);

        await gcaManager
            .ChangeRuleSetCollectionAsync(gcaManager.RuleSetCollection
                .WithoutRuleSet(ruleSet))
            .ConfigureAwait(false);

        gcaSettings.RuleSetModels.Remove(ruleSetModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveRuleSet() => gcaSettings.RuleSetModels.Count > 1;

    private void NotifyCommandsCanExecuteChanged()
    {
        AddRuleSetCommand.NotifyCanExecuteChanged();
        RemoveRuleSetCommand.NotifyCanExecuteChanged();
    }
    #endregion
}