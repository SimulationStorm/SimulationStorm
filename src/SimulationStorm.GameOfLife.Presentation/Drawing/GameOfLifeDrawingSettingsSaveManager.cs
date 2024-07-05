using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Drawing;

public class GameOfLifeDrawingSettingsSaveManager
(
    GameOfLifeDrawingSettings drawingSettings
)
    : DrawingSettingsSaveManager<GameOfLifeCellState, GameOfLifeDrawingSettingsSave>(drawingSettings)
{
    protected override GameOfLifeDrawingSettingsSave SaveServiceCore()
    {
        var state = base.SaveServiceCore();
        state.Pattern = drawingSettings.CurrentPattern;
        state.PlacePatternWithOverlay = drawingSettings.PlacePatternWithOverlay;

        return state;
    }

    protected override void RestoreServiceSaveCore(GameOfLifeDrawingSettingsSave save)
    {
        base.RestoreServiceSaveCore(save);

        drawingSettings.CurrentPattern = save.Pattern;
        drawingSettings.PlacePatternWithOverlay = save.PlacePatternWithOverlay;
    }
}