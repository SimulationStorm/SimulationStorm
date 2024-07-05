using SimulationStorm.AppSaves;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererSaveManager(GameOfLifeRenderer gameOfLifeRenderer) :
    ServiceSaveManagerBase<GameOfLifeRendererSave>
{
    protected override GameOfLifeRendererSave SaveServiceCore() => new()
    {
        IsRenderingEnabled = gameOfLifeRenderer.IsRenderingEnabled,
        RenderingInterval = gameOfLifeRenderer.RenderingInterval,
        DeadCellColor = gameOfLifeRenderer.CellColors.DeadCellColor,
        AliveCellColor = gameOfLifeRenderer.CellColors.AliveCellColor
    };

    protected override void RestoreServiceSaveCore(GameOfLifeRendererSave save)
    {
        gameOfLifeRenderer.IsRenderingEnabled = save.IsRenderingEnabled;
        gameOfLifeRenderer.RenderingInterval = save.RenderingInterval;
        gameOfLifeRenderer.CellColors = (save.DeadCellColor, save.AliveCellColor);
    }
}