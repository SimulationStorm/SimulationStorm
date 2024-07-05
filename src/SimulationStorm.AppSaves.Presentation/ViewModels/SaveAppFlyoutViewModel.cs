using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.AppSaves.Persistence;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;

namespace SimulationStorm.AppSaves.Presentation.ViewModels;

public partial class SaveAppFlyoutViewModel
(
    IAppSaveManager appSaveManager,
    ILocalizedNotificationManager notificationManager,
    AppSavesOptions options
)
    : FlyoutViewModelBase
{
    #region Properties
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "IsRequired")]
    [NotifyCanExecuteChangedFor(nameof(SaveAppCommand))]
    [ObservableProperty]
    private string _appSaveName = string.Empty;

    [ObservableProperty] private bool _isSavingInProgress;
    
    public Range<int> AppSaveNameLengthRange => options.AppSaveNameLengthRange;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveApp))]
    private async Task SaveAppAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        IsBackgroundClosingAllowed = false;
        IsClosingViaCommandAllowed = false;
        IsSavingInProgress = true;
        
        await appSaveManager.SaveAppAsync(AppSaveName.Trim());
        AppSaveName = string.Empty;
        
        IsSavingInProgress = false;
        IsBackgroundClosingAllowed = true;
        IsClosingViaCommandAllowed = true;
        
        notificationManager.ShowSuccess("AppSaves.AppSaved", "Notifications.Notification");
        
        Close();
    }
    private bool CanSaveApp() => !HasErrors;
    #endregion

    public override void OnClosing() => ClearErrors();
}