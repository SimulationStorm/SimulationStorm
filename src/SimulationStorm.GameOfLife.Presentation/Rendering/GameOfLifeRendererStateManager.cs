using SimulationStorm.AppStates;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRendererStateManager(GameOfLifeRenderer gameOfLifeRenderer) :
    ServiceStateManagerBase<GameOfLifeRendererState>
{
    protected override GameOfLifeRendererState SaveServiceStateImpl() => new()
    {
        IsRenderingEnabled = gameOfLifeRenderer.IsRenderingEnabled,
        RenderingInterval = gameOfLifeRenderer.RenderingInterval,
        CellColors = gameOfLifeRenderer.CellColors
    };

    protected override void RestoreServiceStateImpl(GameOfLifeRendererState state)
    {
        gameOfLifeRenderer.IsRenderingEnabled = state.IsRenderingEnabled;
        gameOfLifeRenderer.RenderingInterval = state.RenderingInterval;
        gameOfLifeRenderer.CellColors = state.CellColors;
    }
}