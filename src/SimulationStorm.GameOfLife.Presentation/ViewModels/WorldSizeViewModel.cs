using System.Diagnostics.CodeAnalysis;
using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Bounded.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.ViewModels;

public class WorldSizeViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    GameOfLifeManager gameOfLifeManager,
    ISimulationRunner simulationRunner,
    IWorldCamera worldCamera,
    ILocalizationManager localizationManager,
    INotificationManager notificationManager,
    IBoundedSimulationManagerOptions options
)
    : WorldSizeViewModelBase(uiThreadScheduler, gameOfLifeManager,
        simulationRunner, worldCamera, localizationManager, notificationManager, options)
{
    protected override bool ValidateWorldSize(Size worldSize, [NotNullWhen(false)] out string? errorMessage)
    {
        errorMessage = null;

        if (gameOfLifeManager.Algorithm is GameOfLifeAlgorithm.Bitwise
            && worldSize.Area % BitwiseGameOfLife.BatchSize is not 0)
        {
            errorMessage = localizationManager.GetLocalizedString("GameOfLife.NotAllowedWorldSizeForBitwiseAlgorithm");
        }

        return errorMessage is null;
    }
}