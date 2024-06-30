using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Commands;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.GameOfLife.Presentation.Rules;

public partial class RuleViewModel : DisposableObservableObject
{
    #region Properties
    public GameOfLifeRule ActualRule => _gameOfLifeManager.Rule;
    
    [NotifyCanExecuteChangedFor(nameof(ResetEditingRuleCommand))]
    [NotifyCanExecuteChangedFor(nameof(ApplyRuleCommand))]
    [ObservableProperty]
    private GameOfLifeRule _editingRule;
    #endregion

    #region Commands
    [RelayCommand]
    private void RandomizeEditingRule() => EditingRule = GameOfLifeRule.GenerateRandomRule();

    [RelayCommand(CanExecute = nameof(CanResetEditingRule))]
    private void ResetEditingRule() => EditingRule = GameOfLifeRule.Empty;
    private bool CanResetEditingRule() => !EditingRule.Equals(GameOfLifeRule.Empty);

    [RelayCommand(CanExecute = nameof(CanApplyRule))]
    private async Task ApplyRuleAsync()
    {
        await _gameOfLifeManager.ChangeRuleAsync(EditingRule);
        
        OnPropertyChanged(nameof(ActualRule));
        ApplyRuleCommand.NotifyCanExecuteChanged();
    }
    private bool CanApplyRule() => !EditingRule.Equals(_gameOfLifeManager.Rule);
    #endregion

    private readonly GameOfLifeManager _gameOfLifeManager;

    public RuleViewModel(IUiThreadScheduler uiThreadScheduler, GameOfLifeManager gameOfLifeManager)
    {
        _gameOfLifeManager = gameOfLifeManager;
        _editingRule = gameOfLifeManager.Rule;
        
        WithDisposables(disposables =>
        {
            var executedCommandStream = Observable
                .FromEventPattern<EventHandler<SimulationCommandExecutedEventArgs>, SimulationCommandExecutedEventArgs>
                (
                    h => _gameOfLifeManager.CommandExecuted += h,
                    h => _gameOfLifeManager.CommandExecuted -= h
                )
                .Select(e => e.EventArgs.Command)
                .ObserveOn(uiThreadScheduler);
            
            executedCommandStream
                .Where(command => command is ChangeRuleCommand)
                .Subscribe(_ => OnPropertyChanged(nameof(ActualRule)))
                .DisposeWith(disposables);
            
            executedCommandStream
                .Where(command => command is RestoreStateCommand)
                .Subscribe(_ =>
                {
                    if (EditingRule.Equals(ActualRule))
                        return;

                    OnPropertyChanged(nameof(ActualRule));
                    EditingRule = ActualRule;
                })
                .DisposeWith(disposables);
        });
    }
}