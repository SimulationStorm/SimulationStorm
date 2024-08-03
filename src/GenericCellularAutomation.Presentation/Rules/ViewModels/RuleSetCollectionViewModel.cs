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
    #endregion

    #region Fields
    private readonly ILocalizationManager _localizationManager;
    
    private readonly IRuleSetViewModelFactory _ruleSetViewModelFactory;
    
    private readonly GcaManager _gcaManager;

    private readonly GcaSettings _gcaSettings;
    
    private readonly RulesOptions _rulesOptions;
    
    private readonly ObservableCollection<CellStateModel> _cellStateModels = [];

    private readonly ReadOnlyObservableCollection<CellStateModel> _readOnlyCellStateModels;
    #endregion

    #region Initializing
    public RuleSetCollectionViewModel
    (
        ILocalizationManager localizationManager,
        IRuleSetViewModelFactory ruleSetViewModelFactory,
        GcaManager gcaManager,
        GcaSettings gcaSettings,
        IUiThreadScheduler uiThreadScheduler,
        GcaOptions gcaOptions,
        RulesOptions rulesOptions)
    {
        _localizationManager = localizationManager;
        _ruleSetViewModelFactory = ruleSetViewModelFactory;
        _gcaManager = gcaManager;
        _gcaSettings = gcaSettings;
        _rulesOptions = rulesOptions;
        
        UpdateCellStateModelsFromSettings();
        _readOnlyCellStateModels = new ReadOnlyObservableCollection<CellStateModel>(_cellStateModels);
        
        InitializeRuleSetCollection(gcaOptions.Configuration.RuleSetCollection);

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
            var ruleSetViewModel = CreateRuleSetViewModel();
            ruleSetViewModel.Index = RuleSetViewModels.Count;
            ruleSetViewModel.Name = ruleSetDescriptor.Name;
            ruleSetViewModel.RepetitionCount = ruleSetDescriptor.RepetitionCount;

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

                switch (rule.Type)
                {
                    case RuleType.Totalistic:
                    {
                        ruleModel.NeighborCellCountCollection.Add(rule.NeighborCellCountSet!);
                        break;
                    }
                    case RuleType.Nontotalistic:
                    {
                        ruleModel.NeighborCellPositionCollection.Add(rule.NeighborCellPositionSet!);
                        break;
                    }
                }
                
                ruleSetViewModel.RuleModels.Add(ruleModel);
            });

            RuleSetViewModels.Add(ruleSetViewModel);
        });
        
        return;

        CellStateModel GetCellStateModel(byte cellState) =>
            _cellStateModels.First(csm => csm.CellState == cellState);
    }

    private void UpdateCellStateModelsFromSettings()
    {
        var cellStateDescriptors = _gcaSettings.CellStateCollectionDescriptor.CellStates;

        // Remove old cell state models.
        _cellStateModels
            .ExceptBy(cellStateDescriptors
                .Select(csd => csd.CellState), csm => csm.CellState)
            .ToArray()
            .ForEach(csm =>
                _cellStateModels.Remove(csm));

        // Update existing cell state models.
        cellStateDescriptors.ForEach(csd =>
        {
            var cellStateModel = _cellStateModels
                .FirstOrDefault(csm => csm.CellState == csd.CellState);

            if (cellStateModel is null)
                return;

            cellStateModel.Name = csd.Name;
            cellStateModel.Color = csd.Color;
        });

        // Add new cell state models.
        cellStateDescriptors
            .Where(csd => _cellStateModels
                .All(csm => csd.CellState != csm.CellState))
            .Select(csd => new CellStateModel
            {
                CellState = csd.CellState,
                Name = csd.Name,
                Color = csd.Color
            })
            .ForEach(newCsm =>
                _cellStateModels.Add(newCsm));
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddRuleSet))]
    private void AddRuleSet()
    {
        AddNewRuleSetViewModel();
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddRuleSet() =>
        RuleSetViewModels.Count < _rulesOptions.MaxRuleSetCount;

    [RelayCommand(CanExecute = nameof(CanRemoveRuleSet))]
    private void RemoveRuleSet(RuleSetViewModel ruleSetViewModel)
    {
        RuleSetViewModels.Remove(ruleSetViewModel);
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveRuleSet() =>
        RuleSetViewModels.Count > 1;
    
    [RelayCommand(CanExecute = nameof(CanMoveRuleSetForward))]
    private void MoveRuleSetForward(RuleSetViewModel ruleSetViewModel)
    {
        var newIndex = ruleSetViewModel.Index + 1;
        RuleSetViewModels.Move(ruleSetViewModel.Index, newIndex);
        ruleSetViewModel.Index = newIndex;
        
        NotifyMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveRuleSetForward(RuleSetViewModel ruleSetViewModel) =>
        ruleSetViewModel.Index < RuleSetViewModels.Count - 1;

    [RelayCommand(CanExecute = nameof(CanMoveRuleSetBack))]
    private void MoveRuleSetBack(RuleSetViewModel ruleSetViewModel)
    {
        var newIndex = ruleSetViewModel.Index - 1;
        RuleSetViewModels.Move(ruleSetViewModel.Index, newIndex);
        ruleSetViewModel.Index = newIndex;
        
        NotifyMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveRuleSetBack(RuleSetViewModel ruleSetViewModel) =>
        ruleSetViewModel.Index > 0;

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
        NotifyAddRemoveCommandsCanExecuteChanged();
        NotifyMoveCommandsCanExecuteChanged();
    }
    
    private void NotifyMoveCommandsCanExecuteChanged()
    {
        MoveRuleSetForwardCommand.NotifyCanExecuteChanged();
        MoveRuleSetBackCommand.NotifyCanExecuteChanged();
    }

    private void NotifyAddRemoveCommandsCanExecuteChanged()
    {
        AddRuleSetCommand.NotifyCanExecuteChanged();
        RemoveRuleSetCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Private methods
    private RuleSetCollection BuildRuleSetCollection() => new
    (
        RepetitionCount,
        RuleSetViewModels
            .Select(rsvm => rsvm.ToRuleSet())
            .ToArray()
    );

    #region New rule set creation
    private void AddNewRuleSetViewModel()
    {
        var ruleSetViewModel = CreateRuleSetViewModel();
        ruleSetViewModel.Index = RuleSetViewModels.Count;
        ruleSetViewModel.Name = GetNewRuleSetName();
        ruleSetViewModel.RepetitionCount = _rulesOptions.RuleSetRepetitionCountRange.Minimum;

        ruleSetViewModel.AddNewRuleModel();
        
        RuleSetViewModels.Add(ruleSetViewModel);
    }
    
    private RuleSetViewModel CreateRuleSetViewModel() =>
        _ruleSetViewModelFactory.CreateRuleSetViewModel(_readOnlyCellStateModels);

    private string GetNewRuleSetName() =>
        _localizationManager.GetLocalizedString("Simulation.Gca.NewRuleSet");
    #endregion
    #endregion
}