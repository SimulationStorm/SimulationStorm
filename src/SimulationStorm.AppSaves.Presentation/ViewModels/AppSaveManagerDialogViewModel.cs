using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.AppSaves.Entities;
using SimulationStorm.AppSaves.Presentation.Models;
using SimulationStorm.Dialogs.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.AppSaves.Presentation.ViewModels;

public partial class AppSaveManagerDialogViewModel : DialogViewModelBase
{
    #region Properties
    public ReadOnlyObservableCollection<AppSaveModel> AppSaveModels => _appSaveModels;

    [NotifyCanExecuteChangedFor(nameof(RestoreAppSaveCommand))]
    [ObservableProperty]
    private AppSaveModel? _selectedAppSaveModel;

    public bool CanDeleteAllAppSaves => _appSaveManager.AppSaves.Count > 0;

    [ObservableProperty] private bool _isRestoringInProgress;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanRestoreAppSave))]
    private async Task RestoreAppSaveAsync()
    {
        IsBackgroundClosingAllowed = false;
        IsClosingViaCommandAllowed = false;
        IsRestoringInProgress = true;

        await _appSaveManager.RestoreAppSaveAsync(SelectedAppSaveModel!.AppSave);

        IsRestoringInProgress = false;
        IsClosingViaCommandAllowed = true;
        IsBackgroundClosingAllowed = true;
        
        _notificationManager.ShowSuccess("AppSaves.SaveLoaded", "Notifications.Notification");
        
        Close();
    }
    private bool CanRestoreAppSave() => SelectedAppSaveModel is not null;
    #endregion
    
    #region Fields
    private readonly IAppSaveManager _appSaveManager;
    
    private readonly ILocalizedNotificationManager _notificationManager;

    private readonly ReadOnlyObservableCollection<AppSaveModel> _appSaveModels;
    #endregion

    public AppSaveManagerDialogViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        IAppSaveManager appSaveManager,
        ILocalizedNotificationManager notificationManager)
    {
        _appSaveManager = appSaveManager;
        _notificationManager = notificationManager;
        
        _appSaveManager.AppSaves
            .IndexItemsAndBind<ReadOnlyObservableCollection<AppSave>, AppSave, AppSaveModel>
            (
                appSave => new AppSaveModel(appSave),
                out _appSaveModels,
                uiThreadScheduler
            )
            .DisposeWith(Disposables);

        _appSaveManager.AppSaves
            .ObserveCollectionChanges()
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => OnPropertyChanged(nameof(CanDeleteAllAppSaves)))
            .DisposeWith(Disposables);
    }
}