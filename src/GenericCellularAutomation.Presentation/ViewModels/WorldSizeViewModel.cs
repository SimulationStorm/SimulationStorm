using System.Diagnostics.CodeAnalysis;
using GenericCellularAutomation.Presentation.Management;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Bounded.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Threading.Presentation;

namespace GenericCellularAutomation.Presentation.ViewModels;

public sealed class WorldSizeViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    GcaManager gcaManager,
    ISimulationRunner simulationRunner,
    IWorldCamera worldCamera,
    ILocalizationManager localizationManager,
    INotificationManager notificationManager,
    IBoundedSimulationManagerOptions options
)
    : WorldSizeViewModelBase(uiThreadScheduler, gcaManager,
        simulationRunner, worldCamera, localizationManager, notificationManager, options)
{
    protected override bool ValidateWorldSize(Size worldSize, [NotNullWhen(false)] out string? errorMessage)
    {
        errorMessage = null;

        if (worldSize.Area is 0)
            errorMessage = localizationManager.GetLocalizedString("Gca.WorldSizeAreMustBeGreaterThanZero");

        return errorMessage is null;
    }
}