using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using DynamicData;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Management;
using GenericCellularAutomation.Presentation.Management.Commands;
using GenericCellularAutomation.Presentation.Rules.Descriptors;
using GenericCellularAutomation.Presentation.Rules.Models;
using GenericCellularAutomation.Rules;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace GenericCellularAutomation.Presentation.Rules.ViewModels;

public sealed partial class RuleSetCollectionViewModel : DisposableObservableObject
{
    #region Properties
    [ObservableProperty] private int _repetitionCount;

    public ObservableCollection<RuleSetViewModel> RuleSetViewModels { get; } = [];
    
    /// <summary>
    /// To be used by RuleView's.
    /// </summary>
    public ObservableCollection<CellStateModel> CellStateModels { get; } = [];
    #endregion

    #region Fields
    private readonly GenericCellularAutomationManager _gcaManager;

    private readonly GenericCellularAutomationSettings _gcaSettings;

    private readonly ILocalizationManager _localizationManager;
    
    private readonly RulesOptions _options;
    #endregion

    #region Initializing
    public RuleSetCollectionViewModel
    (
        GenericCellularAutomationManager gcaManager,
        GenericCellularAutomationSettings gcaSettings,
        ILocalizationManager localizationManager,
        IUiThreadScheduler uiThreadScheduler,
        RulesOptions options)
    {
        _gcaManager = gcaManager;
        _gcaSettings = gcaSettings;
        _localizationManager = localizationManager;
        _options = options;
        
        UpdateCellStateModelsFromSettings();
        
        // Todo: create main options object that will contain Configurations and default Configuration:
        // GcaManager will create impl using it, rule set collection view model also will use it for init purposes.
        InitializeRuleSetCollection();

        gcaManager
            .CommandCompletedObservable()
            .Where(e => e.Command is ChangeCellStateCollectionCommand)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => UpdateCellStateModelsFromSettings())
            .DisposeWith(Disposables);
    }

    private void InitializeRuleSetCollection(RuleSetCollectionDescriptor ruleSetCollectionDescriptor)
    {
        RepetitionCount = ruleSetCollectionDescriptor.RepetitionCount;
        
        ruleSetCollectionDescriptor.RuleSets.ForEach(ruleSetDescriptor =>
        {
            var ruleSetViewModel = new RuleSetViewModel
            {
                Index = RuleSetViewModels.Count,
                Name = ruleSetDescriptor.Name,
                RepetitionCount = ruleSetDescriptor.RepetitionCount
            };
            
            ruleSetDescriptor.Rules.ForEach(ruleDescriptor =>
            {
                var rule = ruleDescriptor.Rule;
                
                var ruleModel = new RuleModel
                {
                    Name = ruleDescriptor.Name,
                    Type = rule.Type,
                    ApplicationProbability = rule.ApplicationProbability,
                    TargetCellState = GetCellStateModel(rule.TargetCellState),
                    NewCellState = GetCellStateModel(rule.NewCellState),
                    NeighborCellState = rule.NeighborCellState.HasValue
                        ? GetCellStateModel(rule.NeighborCellState.Value)
                        : null,
                    CellNeighborhood = rule.CellNeighborhood,
                };

                if (rule.Type is RuleType.Totalistic)
                    ruleModel.NeighborCellCountCollection.Add(rule.NeighborCellCountSet!);
                else if (rule.Type is RuleType.Nontotalistic)
                    ruleModel.NeighborCellPositionCollection.Add(rule.NeighborCellPositionSet!);
                
                ruleSetViewModel.RuleModels.Add(ruleModel);
            });

            RuleSetViewModels.Add(ruleSetViewModel);
        });
        
        return;

        CellStateModel GetCellStateModel(byte cellState) =>
            CellStateModels.First(csm => csm.CellState == cellState);
    }

    private void UpdateCellStateModelsFromSettings()
    {
        var cellStateDescriptors = _gcaSettings.CellStateCollectionDescriptor.CellStates;

        // Remove old cell state models.
        CellStateModels
            .ExceptBy(cellStateDescriptors
                .Select(csd => csd.CellState), csm => csm.CellState)
            .ToArray()
            .ForEach(csm => CellStateModels.Remove(csm));

        // Update existing cell state models.
        cellStateDescriptors.ForEach(csd =>
        {
            var cellStateModel = CellStateModels
                .FirstOrDefault(csm => csm.CellState == csd.CellState);

            if (cellStateModel is null)
                return;

            cellStateModel.Name = csd.Name;
            cellStateModel.Color = csd.Color;
        });

        // Add new cell state models.
        cellStateDescriptors
            .Where(csd => CellStateModels
                .All(csm => csd.CellState != csm.CellState))
            .Select(csd => new CellStateModel
            {
                CellState = csd.CellState,
                Name = csd.Name,
                Color = csd.Color
            })
            .ForEach(newCsm => CellStateModels.Add(newCsm));
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddRuleSet))]
    private void AddRuleSet()
    {
        var ruleSetViewModel = new RuleSetViewModel
        {
            Index = RuleSetViewModels.Count,
            Name = GetNewRuleSetName(),
            RepetitionCount = _options.RuleSetRepetitionCountRange.Minimum
        };

        // Todo: what about creating default rule model?
        
        RuleSetViewModels.Add(ruleSetViewModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddRuleSet() => RuleSetViewModels.Count < _options.MaxRuleSetCount;

    [RelayCommand(CanExecute = nameof(CanRemoveRuleSet))]
    private void RemoveRuleSet(RuleSetViewModel ruleSetViewModel)
    {
        RuleSetViewModels.Remove(ruleSetViewModel);
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveRuleSet() => RuleSetViewModels.Count > 1;

    [RelayCommand]
    private Task ApplyChangesAsync() => Task.Run(async () =>
    {
        var ruleSetCollection = BuildRuleSetCollection();
        
        await _gcaManager
            .ChangeRuleSetCollectionAsync(ruleSetCollection)
            .ConfigureAwait(false);
    });

    private void NotifyCommandsCanExecuteChanged()
    {
        AddRuleSetCommand.NotifyCanExecuteChanged();
        RemoveRuleSetCommand.NotifyCanExecuteChanged();
    }
    #endregion

    private RuleSetCollection BuildRuleSetCollection() => new
    (
        RepetitionCount,
        RuleSetViewModels
            .Select(rsvm => rsvm.ToRuleSet())
            .ToArray()
    );

    private string GetNewRuleSetName() =>
        _localizationManager.GetLocalizedString("Simulation.Gca.NewRuleSet");
}