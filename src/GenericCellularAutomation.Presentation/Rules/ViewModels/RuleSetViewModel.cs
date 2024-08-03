using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Rules.Models;
using GenericCellularAutomation.Rules;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules.ViewModels;

public sealed partial class RuleSetViewModel : NamedIndexedObservableObject
{
    #region Properties
    [ObservableProperty] private int _repetitionCount;

    public ObservableCollection<RuleModel> RuleModels { get; } = [];

    public ReadOnlyObservableCollection<CellStateModel> CellStateModels { get; }
    #endregion

    #region Fields
    private readonly ILocalizationManager _localizationManager;

    private readonly RulesOptions _options;
    #endregion

    public RuleSetViewModel
    (
        ILocalizationManager localizationManager,
        RulesOptions options,
        ReadOnlyObservableCollection<CellStateModel> cellStateModels)
    {
        _localizationManager = localizationManager;
        _options = options;
        
        CellStateModels = cellStateModels;
    }
    
    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddRule))]
    private void AddRule()
    {
        AddNewRuleModel();
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddRule() => RuleModels.Count < _options.MaxRuleCount;

    [RelayCommand(CanExecute = nameof(CanRemoveRule))]
    private void RemoveRule(RuleModel ruleModel)
    {
        RuleModels.Remove(ruleModel);
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveRule() => RuleModels.Count > 1;

    [RelayCommand(CanExecute = nameof(CanMoveRuleForward))]
    private void MoveRuleForward(RuleModel ruleModel)
    {
        var newIndex = ruleModel.Index + 1;
        RuleModels.Move(ruleModel.Index, newIndex);
        ruleModel.Index = newIndex;
        
        NotifyMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveRuleForward(RuleModel ruleModel) =>
        ruleModel.Index < RuleModels.Count - 1;

    [RelayCommand(CanExecute = nameof(CanMoveRuleBack))]
    private void MoveRuleBack(RuleModel ruleModel)
    {
        var newIndex = ruleModel.Index - 1;
        RuleModels.Move(ruleModel.Index, newIndex);
        ruleModel.Index = newIndex;
        
        NotifyMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveRuleBack(RuleModel ruleModel) =>
        ruleModel.Index > 0;

    private void NotifyCommandsCanExecuteChanged()
    {
        NotifyAddRemoveCommandsCanExecuteChanged();
        NotifyMoveCommandsCanExecuteChanged();
    }
    
    private void NotifyMoveCommandsCanExecuteChanged()
    {
        MoveRuleForwardCommand.NotifyCanExecuteChanged();
        MoveRuleBackCommand.NotifyCanExecuteChanged();
    }

    private void NotifyAddRemoveCommandsCanExecuteChanged()
    {
        AddRuleCommand.NotifyCanExecuteChanged();
        RemoveRuleCommand.NotifyCanExecuteChanged();
    }
    #endregion
    
    public RuleSet ToRuleSet() => new
    (
        RepetitionCount,
        RuleModels
            .Select(rm => rm.ToRule())
            .ToArray()
    );

    public void AddNewRuleModel() =>
        RuleModels.Add(CreateRuleModel());

    #region Private methods
    #region New rule model creation
    private RuleModel CreateRuleModel() => new()
    {
        Index = RuleModels.Count,
        Name = GetNewRuleName(),
        ApplicationProbability = 1,
        Type = RuleType.Unconditional,
        TargetCellState = GetDefaultCellStateModel(),
        NewCellState = GetDefaultCellStateModel(),
        CellNeighborhood = new CellNeighborhood(1, new HashSet<Point>())
    };

    private string GetNewRuleName() =>
        _localizationManager.GetLocalizedString("Gca.NewRule");

    private CellStateModel GetDefaultCellStateModel() =>
        CellStateModels[0];
    #endregion
    #endregion
}