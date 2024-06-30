using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Commands;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.GameOfLife.Presentation.ViewModels;

public partial class AlgorithmViewModel : DisposableObservableObject
{
    #region Properties
    public GameOfLifeAlgorithm ActualAlgorithm => _gameOfLifeManager.Algorithm;
    
    [NotifyCanExecuteChangedFor(nameof(ChangeAlgorithmCommand))]
    [ObservableProperty]
    private GameOfLifeAlgorithm _selectedAlgorithm;

    public IEnumerable<GameOfLifeAlgorithm> Algorithms { get; } = new[]
    {
        GameOfLifeAlgorithm.Bitwise,
        GameOfLifeAlgorithm.Smart
    };
    #endregion

    #region Fields
    private readonly GameOfLifeManager _gameOfLifeManager;

    private readonly ILocalizedNotificationManager _notificationManager;
    #endregion

    public AlgorithmViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        GameOfLifeManager gameOfLifeManager,
        ILocalizedNotificationManager notificationManager)
    {
        _gameOfLifeManager = gameOfLifeManager;
        _notificationManager = notificationManager;
        _selectedAlgorithm = gameOfLifeManager.Algorithm;
        
        WithDisposables(disposables =>
        {
            var executedCommandStream = Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => _gameOfLifeManager.CommandCompleted += h,
                    h => _gameOfLifeManager.CommandCompleted -= h
                )
                .Select(e => e.EventArgs.Command)
                .ObserveOn(uiThreadScheduler);
                
            executedCommandStream
                .Where(command => command is ChangeAlgorithmCommand)
                .Subscribe(_ => OnPropertyChanged(nameof(ActualAlgorithm)))
                .DisposeWith(disposables);
            
            executedCommandStream
                .Where(command => command is RestoreStateCommand)
                .Subscribe(_ =>
                {
                    if (SelectedAlgorithm == ActualAlgorithm)
                        return;

                    OnPropertyChanged(nameof(ActualAlgorithm));
                    SelectedAlgorithm = ActualAlgorithm;
                })
                .DisposeWith(disposables);
        });
    }

    [RelayCommand(CanExecute = nameof(CanChangeAlgorithm))]
    private async Task ChangeAlgorithmAsync()
    {
        if (SelectedAlgorithm is GameOfLifeAlgorithm.Bitwise)
        {
            var isValidWorldSize = _gameOfLifeManager.WorldSize.Area % BitwiseGameOfLife.BatchSize is 0;
            if (!isValidWorldSize)
            {
                _notificationManager.ShowError(
                    "GameOfLife.NotAllowedWorldSizeForBitwiseAlgorithm", "Notifications.Error");

                return;
            }
        }
        
        await _gameOfLifeManager.ChangeAlgorithmAsync(SelectedAlgorithm);
        OnPropertyChanged(nameof(ActualAlgorithm));
    }
    private bool CanChangeAlgorithm() => SelectedAlgorithm != _gameOfLifeManager.Algorithm;
}