using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Commands;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

public abstract partial class WorldSizeViewModelBase : DisposableObservableObject, IWorldSizeViewModel
{
    #region Properties
    public Size ActualWorldSize => _simulationManager.WorldSize;
    
    public int EditingWorldWidth
    {
        get => _editingWorldSize.Width;
        set
        {
            if (value == _editingWorldSize.Width)
                return;
            
            _editingWorldSize = new Size(value, _editingWorldSize.Height);
            OnPropertyChanged();

            if (KeepAspectRatio)
                EditingWorldHeight = (int)Math.Round(value / _widthToHeightRatio);
            
            ChangeWorldSizeCommand.NotifyCanExecuteChanged();
        }
    }
    
    public int EditingWorldHeight
    {
        get => _editingWorldSize.Height;
        set
        {
            if (value == _editingWorldSize.Height)
                return;
            
            _editingWorldSize = new Size(_editingWorldSize.Width, value);
            OnPropertyChanged();
            
            if (KeepAspectRatio)
                EditingWorldWidth = (int)Math.Round(value * _widthToHeightRatio);
            
            ChangeWorldSizeCommand.NotifyCanExecuteChanged();
        }
    }

    public Range<Size> WorldSizeRange { get; }

    public bool KeepAspectRatio
    {
        get => _keepAspectRatio;
        set => _uiThreadScheduler.Schedule(() =>
        {
            if (SetProperty(ref _keepAspectRatio, value))
                _widthToHeightRatio = (double)EditingWorldWidth / EditingWorldHeight;
        });
    }

    private double _widthToHeightRatio;
    #endregion

    [RelayCommand(CanExecute = nameof(CanChangeWorldSize))]
    private async Task ChangeWorldSizeAsync()
    {
        if (!ValidateWorldSize(_editingWorldSize, out var errorMessage))
        {
            _notificationManager.ShowError
            (
                errorMessage,
                _localizationManager.GetLocalizedString("Notifications.Error")
            );
            
            return;
        }
        
        _simulationRunner.PauseSimulation();
        await _simulationManager.ChangeWorldSizeAsync(_editingWorldSize);
        _worldCamera.ResetTranslation();
        
        OnPropertyChanged(nameof(ActualWorldSize));
        ChangeWorldSizeCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeWorldSize() => _editingWorldSize != ActualWorldSize;

    #region Fields
    private readonly IUiThreadScheduler _uiThreadScheduler;
    
    private readonly IBoundedSimulationManager _simulationManager;
    
    private readonly ISimulationRunner _simulationRunner;
    
    private readonly IWorldCamera _worldCamera;

    private readonly ILocalizationManager _localizationManager;
    
    private readonly INotificationManager _notificationManager;
    
    private Size _editingWorldSize;

    private bool _keepAspectRatio;
    #endregion

    protected WorldSizeViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        IBoundedSimulationManager simulationManager,
        ISimulationRunner simulationRunner,
        IWorldCamera worldCamera,
        ILocalizationManager localizationManager,
        INotificationManager notificationManager,
        IBoundedSimulationManagerOptions options)
    {
        _uiThreadScheduler = uiThreadScheduler;
        
        _simulationManager = simulationManager;
        _simulationRunner = simulationRunner;
        _worldCamera = worldCamera;
        _localizationManager = localizationManager;
        _notificationManager = notificationManager;
        
        _editingWorldSize = simulationManager.WorldSize;
        WorldSizeRange = options.WorldSizeRange;
        
        WithDisposables(disposables =>
        {
            var executedCommandStream = Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => _simulationManager.CommandCompleted += h,
                    h => _simulationManager.CommandCompleted -= h
                )
                .Select(e => e.EventArgs.Command)
                .ObserveOn(uiThreadScheduler);
                
            executedCommandStream
                .Where(command => command is ChangeWorldSizeCommand)
                .Subscribe(_ => OnPropertyChanged(nameof(ActualWorldSize)))
                .DisposeWith(disposables);
            
            executedCommandStream
                .Where(command => command is RestoreStateCommand)
                .Subscribe(_ =>
                {
                    if (_editingWorldSize == ActualWorldSize)
                        return;

                    OnPropertyChanged(nameof(ActualWorldSize));
                    
                    _editingWorldSize = ActualWorldSize;
                    OnPropertyChanged(nameof(EditingWorldWidth));
                    OnPropertyChanged(nameof(EditingWorldHeight));
                })
                .DisposeWith(disposables);
        });
    }

    protected virtual bool ValidateWorldSize(Size worldSize, [NotNullWhen(false)] out string? errorMessage)
    {
        errorMessage = null;
        return true;
    }
}