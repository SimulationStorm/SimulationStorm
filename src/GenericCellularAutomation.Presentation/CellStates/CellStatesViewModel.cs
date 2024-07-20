using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Management;
using GenericCellularAutomation.Presentation.Management.Commands;
using SimulationStorm.Graphics;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Disposing;

namespace GenericCellularAutomation.Presentation.CellStates;

public partial class CellStatesViewModel : DisposableObservableObject
{
    #region Properties
    [ObservableProperty] private bool _randomizeNewCellStateColor;

    public ObservableCollection<CellStateModel> CellStateModels { get; } = [];

    public Range<int> CellStateNameLengthRange => _options.CellStateNameLengthRange;
    #endregion

    #region Fields
    private readonly GenericCellularAutomationManager _gcaManager;

    private readonly GenericCellularAutomationSettings _gcaSettings;

    private readonly ILocalizationManager _localizationManager;
    
    private readonly CellStatesOptions _options;
    #endregion
    
    public CellStatesViewModel
    (
        GenericCellularAutomationManager gcaManager,
        GenericCellularAutomationSettings gcaSettings,
        ILocalizationManager localizationManager,
        CellStatesOptions options)
    {
        _gcaManager = gcaManager;
        _gcaSettings = gcaSettings;
        _localizationManager = localizationManager;
        _options = options;

        _gcaManager
            .CommandCompletedObservable()
            .Where(e => e.Command is ChangeRuleSetCollectionCommand)
            .Subscribe(_ => NotifyCommandsCanExecuteChanged())
            .DisposeWith(Disposables);
    }
    
    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddCellState))]
    private void AddCellState()
    {
        var cellStateModel = new CellStateModel
        {
            Index = CellStateModels.Count,
            CellState = GetNewCellState(),
            Name = GetNewCellStateName(),
            Color = RandomizeNewCellStateColor ? ColorUtils.GenerateRandomColor() : default
        };

        CellStateModels.Add(cellStateModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanAddCellState() => CellStateModels.Count < _options.MaxCellStateCount;

    [RelayCommand(CanExecute = nameof(CanRemoveCellState))]
    private void RemoveCellState(CellStateModel cellStateModel)
    {
        CellStateModels.Remove(cellStateModel);
        
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanRemoveCellState(CellStateModel cellStateModel) =>
        CellStateModels.Count > 1
        && !cellStateModel.IsDefault
        && !_gcaManager.RuleSetCollection.UsedCellStates.Contains(cellStateModel.CellState);

    [RelayCommand(CanExecute = nameof(CanMarkCellStateAsDefault))]
    private void MarkCellStateAsDefault(CellStateModel cellStateModel)
    {
        CellStateModels
            .Where(csm => csm != cellStateModel)
            .ForEach(csm => csm.IsDefault = false);
        
        cellStateModel.IsDefault = true;
        
        NotifyCommandsCanExecuteChanged();
    }
    private static bool CanMarkCellStateAsDefault(CellStateModel cellStateModel) => !cellStateModel.IsDefault;

    // Todo: observe all cell state models and notify apply changes can execute changed
    [RelayCommand(CanExecute = nameof(CanApplyChanges))]
    private async Task ApplyChangesAsync()
    {
        var cellStateCollectionDescriptor = BuildCellStateCollectionDescriptor();
        
        _gcaSettings.CellStateCollectionDescriptor = cellStateCollectionDescriptor;
        
        await _gcaManager
            .ChangeCellStateCollectionAsync(cellStateCollectionDescriptor
                .ToCellStateCollection())
            .ConfigureAwait(false);
    }
    private bool CanApplyChanges()
    {
        
    }
    
    private void NotifyCommandsCanExecuteChanged()
    {
        AddCellStateCommand.NotifyCanExecuteChanged();
        RemoveCellStateCommand.NotifyCanExecuteChanged();
        MarkCellStateAsDefaultCommand.NotifyCanExecuteChanged();
        ApplyChangesCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Helpers
    private byte GetNewCellState()
    {
        var cellStateRange = _gcaManager.CellStateRange;
        
        for (var cellState = cellStateRange.Minimum; cellState <= cellStateRange.Maximum; cellState++)
        {
            if (CellStateModels.All(csm => csm.CellState != cellState))
                return cellState;
        }

        throw new InvalidOperationException("There is no more unoccupied cell state number.");
    }

    private string GetNewCellStateName() =>
        _localizationManager.GetLocalizedString("Simulation.GCA.NewCellState");
    
    private CellStateCollectionDescriptor BuildCellStateCollectionDescriptor()
    {
        var cellStateCollectionDescriptorBuilder = new CellStateCollectionDescriptorBuilder();
        
        CellStateModels.ForEach(cellStateModel =>
        {
            var cellStateDescriptor = new CellStateDescriptorBuilder()
                .HasCellState(cellStateModel.CellState)
                .HasName(cellStateModel.Name)
                .HasColor(cellStateModel.Color)
                .Build();

            if (cellStateModel.IsDefault)
                cellStateCollectionDescriptorBuilder.HasDefaultCellState(cellStateDescriptor);
            else
                cellStateCollectionDescriptorBuilder.HasCellState(cellStateDescriptor);
        });

        return cellStateCollectionDescriptorBuilder.Build();
    }
    #endregion
}