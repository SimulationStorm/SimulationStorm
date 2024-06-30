using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.AppStates.Persistence;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;

namespace SimulationStorm.AppStates.Presentation.ViewModels;

public partial class SaveAppStateFlyoutViewModel
(
    IAppStateManager appStateManager,
    ILocalizedNotificationManager notificationManager,
    AppStatesOptions options
)
    : FlyoutViewModelBase
{
    #region Properties
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "IsRequired")]
    [NotifyCanExecuteChangedFor(nameof(SaveAppStateCommand))]
    [ObservableProperty]
    private string _appStateName = string.Empty;

    [ObservableProperty] private bool _isSavingInProgress;
    
    public Range<int> AppStateNameLengthRange => options.AppStateNameLengthRange;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveAppState))]
    private async Task SaveAppStateAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        IsBackgroundClosingAllowed = false;
        IsClosingViaCommandAllowed = false;
        IsSavingInProgress = true;
        
        await appStateManager.SaveAppStateAsync(AppStateName.Trim());
        AppStateName = string.Empty;
        
        IsSavingInProgress = false;
        IsBackgroundClosingAllowed = true;
        IsClosingViaCommandAllowed = true;
        
        notificationManager.ShowSuccess("AppStates.AppSaved", "Notifications.Notification");
        
        Close();
    }
    private bool CanSaveAppState() => !HasErrors;
    #endregion

    public override void OnClosing() => ClearErrors();
}