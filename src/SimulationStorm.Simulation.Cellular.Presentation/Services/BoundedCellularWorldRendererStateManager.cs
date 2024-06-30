using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public class BoundedCellularWorldRendererStateManager(IBoundedCellularWorldRenderer worldRenderer) :
    ServiceStateManagerBase<BoundedCellularWorldRendererState>
{
    protected override BoundedCellularWorldRendererState SaveServiceStateImpl() => new()
    {
        BackgroundColor = worldRenderer.BackgroundColor,
        IsGridLinesVisible = worldRenderer.IsGridLinesVisible,
        GridLinesColor = worldRenderer.GridLinesColor
    };

    protected override void RestoreServiceStateImpl(BoundedCellularWorldRendererState state)
    {
        worldRenderer.BackgroundColor = state.BackgroundColor;
        worldRenderer.IsGridLinesVisible = state.IsGridLinesVisible;
        worldRenderer.GridLinesColor = state.GridLinesColor;
    }
}