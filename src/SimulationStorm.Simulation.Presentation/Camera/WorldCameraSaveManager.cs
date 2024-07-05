using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class WorldCameraSaveManager(IWorldCamera worldCamera) : ServiceSaveManagerBase<WorldCameraSave>
{
    protected override WorldCameraSave SaveServiceCore() => new(worldCamera.Zoom, worldCamera.Translation);

    protected override void RestoreServiceSaveCore(WorldCameraSave save)
    {
        worldCamera.ZoomToViewportCenter(save.Zoom);
        worldCamera.Translate(save.Translation);
    }
}