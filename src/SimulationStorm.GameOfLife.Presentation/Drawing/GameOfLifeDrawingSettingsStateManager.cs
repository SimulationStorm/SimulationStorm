using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public class GameOfLifeDrawingSettingsStateManager
(
    GameOfLifeDrawingSettings drawingSettings
)
    : DrawingSettingsStateManager<GameOfLifeCellState, GameOfLifeDrawingSettingsState>(drawingSettings)
{
    protected override GameOfLifeDrawingSettingsState SaveServiceStateImpl()
    {
        var state = base.SaveServiceStateImpl();
        state.Pattern = drawingSettings.CurrentPattern;
        state.PlacePatternWithOverlay = drawingSettings.PlacePatternWithOverlay;

        return state;
    }

    protected override void RestoreServiceStateImpl(GameOfLifeDrawingSettingsState state)
    {
        base.RestoreServiceStateImpl(state);

        drawingSettings.CurrentPattern = state.Pattern;
        drawingSettings.PlacePatternWithOverlay = state.PlacePatternWithOverlay;
    }
}