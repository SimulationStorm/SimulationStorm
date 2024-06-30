using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class WorldCameraStateManager(IWorldCamera worldCamera) : ServiceStateManagerBase<WorldCameraState>
{
    protected override WorldCameraState SaveServiceStateImpl() => new(worldCamera.Zoom, worldCamera.Translation);

    protected override void RestoreServiceStateImpl(WorldCameraState state)
    {
        worldCamera.ZoomToViewportCenter(state.Zoom);
        worldCamera.Translate(state.Translation);
    }
}