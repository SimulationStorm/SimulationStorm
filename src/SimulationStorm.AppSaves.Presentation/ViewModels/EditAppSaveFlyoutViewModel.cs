using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.AppSaves.Persistence;
using SimulationStorm.AppSaves.Presentation.Models;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;

namespace SimulationStorm.AppSaves.Presentation.ViewModels;

public partial class EditAppSaveFlyoutViewModel
(
    IAppSaveManager appSaveManager,
    ILocalizedNotificationManager notificationManager,
    AppSavesOptions options
)
    : FlyoutViewModelBase
{
    #region Properties
    [ObservableProperty] private AppSaveModel? _appSaveModel;

    [Required(ErrorMessage = "IsRequired")]
    [CustomValidation(typeof(EditAppSaveFlyoutViewModel), nameof(ValidateEditingAppSaveNameForUniqueness))]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    [ObservableProperty]
    private string _editingAppSaveName = string.Empty;
    
    public Range<int> AppSaveNameLengthRange => options.AppSaveNameLengthRange;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private async Task SaveChangesAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        AppSaveModel!.Name = EditingAppSaveName.Trim();
        await appSaveManager.UpdateAppSaveAsync(AppSaveModel.AppSave);
        
        notificationManager.ShowSuccess("AppSaves.ChangesSaved", "Notifications.Notification");
        
        Close();
    }
    private bool CanSaveChanges() => !HasErrors;

    [RelayCommand]
    private async Task DeleteAppSaveAsync()
    {
        await appSaveManager.DeleteAppSaveAsync(AppSaveModel!.AppSave);
        
        notificationManager.ShowInformation("AppSaves.SaveDeleted", "Notifications.Notification");
        
        Close();
    }
    #endregion

    partial void OnAppSaveModelChanged(AppSaveModel? value)
    {
        EditingAppSaveName = value?.Name ?? string.Empty;
        ClearErrors();
    }

    public override void OnClosing() => AppSaveModel = null;

    public static ValidationResult ValidateEditingAppSaveNameForUniqueness(string editingAppSaveName, ValidationContext context)
    {
        var instance = (EditAppSaveFlyoutViewModel)context.ObjectInstance;

        return editingAppSaveName != instance.AppSaveModel!.Name
            ? ValidationResult.Success!
            : new ValidationResult("ShouldBeDifferent");
    }
}