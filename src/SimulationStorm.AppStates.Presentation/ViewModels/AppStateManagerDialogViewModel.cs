using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.AppStates.Presentation.Models;
using SimulationStorm.Dialogs.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.AppStates.Presentation.ViewModels;

public partial class AppStateManagerDialogViewModel : DialogViewModelBase
{
    #region Properties
    public ReadOnlyObservableCollection<AppStateModel> AppStateModels => _appStateModels;

    [NotifyCanExecuteChangedFor(nameof(RestoreAppStateCommand))]
    [ObservableProperty]
    private AppStateModel? _selectedAppStateModel;

    public bool CanDeleteAllAppStates => _appStateManager.AppStates.Count > 0;

    [ObservableProperty] private bool _isRestoringInProgress;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanRestoreAppState))]
    private async Task RestoreAppStateAsync()
    {
        IsBackgroundClosingAllowed = false;
        IsClosingViaCommandAllowed = false;
        IsRestoringInProgress = true;

        await _appStateManager.RestoreAppStateAsync(SelectedAppStateModel!.AppState);

        IsRestoringInProgress = false;
        IsClosingViaCommandAllowed = true;
        IsBackgroundClosingAllowed = true;
        
        _notificationManager.ShowSuccess("AppStates.SaveLoaded", "Notifications.Notification");
        
        Close();
    }
    private bool CanRestoreAppState() => SelectedAppStateModel is not null;
    #endregion
    
    #region Fields
    private readonly IAppStateManager _appStateManager;
    
    private readonly ILocalizedNotificationManager _notificationManager;

    private ReadOnlyObservableCollection<AppStateModel> _appStateModels = null!;
    #endregion

    public AppStateManagerDialogViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        IAppStateManager appStateManager,
        ILocalizedNotificationManager notificationManager)
    {
        _appStateManager = appStateManager;
        _notificationManager = notificationManager;
        
        _appStateManager.AppStates
            .IndexItemsAndBind<ReadOnlyObservableCollection<AppState>, AppState, AppStateModel>
            (
                appState => new AppStateModel(appState),
                out _appStateModels,
                uiThreadScheduler
            )
            .DisposeWith(Disposables);

        _appStateManager.AppStates
            .ObserveCollectionChanges()
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => OnPropertyChanged(nameof(CanDeleteAllAppStates)))
            .DisposeWith(Disposables);
    }
}