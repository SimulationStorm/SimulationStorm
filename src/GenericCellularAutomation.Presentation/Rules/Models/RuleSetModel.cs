using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Rules.Models;

public sealed partial class RuleSetModel : NamedIndexedObservableObject
{
    [ObservableProperty] private int _repetitionCount;

    public ObservableCollection<RuleModel> RuleModels { get; } = [];
}