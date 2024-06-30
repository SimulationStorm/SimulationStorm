using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Flyouts.Presentation;
using SimulationStorm.Notifications.Presentation;

namespace SimulationStorm.AppStates.Presentation.ViewModels;

public partial class DeleteAllAppStatesFlyoutViewModel
(
    IAppStateManager appStateManager,
    ILocalizedNotificationManager notificationManager
)
    : FlyoutViewModelBase
{
    [RelayCommand]
    private async Task DeleteAllAppStatesAsync()
    {
        await appStateManager.DeleteAllAppStatesAsync();
        
        notificationManager.ShowInformation("AppStates.AllSavesDeleted", "Notifications.Notification");
        
        Close();
    }
}