using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

public class WorldSizeViewModelSaveManager(IWorldSizeViewModel worldSizeViewModel) :
    ServiceSaveManagerBase<WorldSizeViewModelSave>
{
    protected override WorldSizeViewModelSave SaveServiceCore() => new()
    {
        KeepAspectRatio = worldSizeViewModel.KeepAspectRatio
    };

    protected override void RestoreServiceSaveCore(WorldSizeViewModelSave save) =>
        worldSizeViewModel.KeepAspectRatio = save.KeepAspectRatio;
}