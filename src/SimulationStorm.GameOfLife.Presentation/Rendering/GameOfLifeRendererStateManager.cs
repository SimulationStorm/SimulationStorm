using SimulationStorm.AppSaves;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererSaveManager(GameOfLifeRenderer gameOfLifeRenderer) :
    ServiceSaveManagerBase<GameOfLifeRendererSave>
{
    protected override GameOfLifeRendererSave SaveServiceCore() => new()
    {
        IsRenderingEnabled = gameOfLifeRenderer.IsRenderingEnabled,
        RenderingInterval = gameOfLifeRenderer.RenderingInterval,
        CellColors = gameOfLifeRenderer.CellColors
    };

    protected override void RestoreServiceSaveCore(GameOfLifeRendererSave save)
    {
        gameOfLifeRenderer.IsRenderingEnabled = save.IsRenderingEnabled;
        gameOfLifeRenderer.RenderingInterval = save.RenderingInterval;
        gameOfLifeRenderer.CellColors = save.CellColors;
    }
}