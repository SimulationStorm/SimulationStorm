using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

public class WorldSizeViewModelStateManager(IWorldSizeViewModel worldSizeViewModel) :
    ServiceStateManagerBase<WorldSizeViewModelState>
{
    protected override WorldSizeViewModelState SaveServiceStateImpl() => new()
    {
        KeepAspectRatio = worldSizeViewModel.KeepAspectRatio
    };

    protected override void RestoreServiceStateImpl(WorldSizeViewModelState state) =>
        worldSizeViewModel.KeepAspectRatio = state.KeepAspectRatio;
}