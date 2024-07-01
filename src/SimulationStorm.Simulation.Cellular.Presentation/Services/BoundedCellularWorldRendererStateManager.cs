using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public class BoundedCellularWorldRendererSaveManager(IBoundedCellularWorldRenderer worldRenderer) :
    ServiceSaveManagerBase<BoundedCellularWorldRendererSave>
{
    protected override BoundedCellularWorldRendererSave SaveServiceCore() => new()
    {
        BackgroundColor = worldRenderer.BackgroundColor,
        IsGridLinesVisible = worldRenderer.IsGridLinesVisible,
        GridLinesColor = worldRenderer.GridLinesColor
    };

    protected override void RestoreServiceSaveCore(BoundedCellularWorldRendererSave save)
    {
        worldRenderer.BackgroundColor = save.BackgroundColor;
        worldRenderer.IsGridLinesVisible = save.IsGridLinesVisible;
        worldRenderer.GridLinesColor = save.GridLinesColor;
    }
}