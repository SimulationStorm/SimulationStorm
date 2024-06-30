using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.AppStates.Persistence;
using SimulationStorm.AppStates.Presentation.Models;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;

namespace SimulationStorm.AppStates.Presentation.ViewModels;

public partial class EditAppStateFlyoutViewModel
(
    IAppStateManager appStateManager,
    ILocalizedNotificationManager notificationManager,
    AppStatesOptions options
)
    : FlyoutViewModelBase
{
    #region Properties
    [ObservableProperty] private AppStateModel? _appStateModel;

    [Required(ErrorMessage = "IsRequired")]
    [CustomValidation(typeof(EditAppStateFlyoutViewModel), nameof(ValidateEditingAppStateNameForUniqueness))]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    [ObservableProperty]
    private string _editingAppStateName = string.Empty;
    
    public Range<int> AppStateNameLengthRange => options.AppStateNameLengthRange;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private async Task SaveChangesAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        AppStateModel!.Name = EditingAppStateName.Trim();
        await appStateManager.UpdateAppStateAsync(AppStateModel.AppState);
        
        notificationManager.ShowSuccess("AppStates.ChangesSaved", "Notifications.Notification");
        
        Close();
    }
    private bool CanSaveChanges() => !HasErrors;

    [RelayCommand]
    private async Task DeleteAppStateAsync()
    {
        await appStateManager.DeleteAppStateAsync(AppStateModel!.AppState);
        
        notificationManager.ShowInformation("AppStates.SaveDeleted", "Notifications.Notification");
        
        Close();
    }
    #endregion

    partial void OnAppStateModelChanged(AppStateModel? value)
    {
        EditingAppStateName = value?.Name ?? string.Empty;
        ClearErrors();
    }

    public override void OnClosing() => AppStateModel = null;

    public static ValidationResult ValidateEditingAppStateNameForUniqueness(string editingAppStateName, ValidationContext context)
    {
        var instance = (EditAppStateFlyoutViewModel)context.ObjectInstance;

        return editingAppStateName != instance.AppStateModel!.Name
            ? ValidationResult.Success!
            : new ValidationResult("ShouldBeDifferent");
    }
}