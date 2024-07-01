using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;

namespace SimulationStorm.AppSaves.Presentation.ViewModels;

public partial class DeleteAllAppSavesFlyoutViewModel
(
    IAppSaveManager appSaveManager,
    ILocalizedNotificationManager notificationManager
)
    : FlyoutViewModelBase
{
    [RelayCommand]
    private async Task DeleteAllAppSavesAsync()
    {
        await appSaveManager.DeleteAllAppSavesAsync();
        
        notificationManager.ShowInformation("AppSaves.AllSavesDeleted", "Notifications.Notification");
        
        Close();
    }
}