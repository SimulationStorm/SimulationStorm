using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Rules.Models;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.ViewModels;

public sealed partial class RuleSetViewModel : NamedIndexedObservableObject
{
    #region Properties
    [ObservableProperty] private int _repetitionCount;

    public ObservableCollection<RuleModel> RuleModels { get; } = [];
    #endregion

    public RuleSet ToRuleSet() => new
    (
        RepetitionCount,
        RuleModels
            .Select(rm => rm.ToRule())
            .ToArray()
    );
}