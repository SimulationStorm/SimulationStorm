using SimulationStorm.AppSaves;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public class DrawingSettingsSaveManager<TCellState, TSave>
(
    IDrawingSettings<TCellState> drawingSettings
)
    : ServiceSaveManagerBase<TSave>
    where TSave : DrawingSettingsSave<TCellState>, new()
{
    protected override TSave SaveServiceCore() => new()
    {
        IsDrawingEnabled = drawingSettings.IsDrawingEnabled,
        DrawingBrushShape = drawingSettings.BrushShape,
        DrawingBrushRadius = drawingSettings.BrushRadius,
        DrawingBrushCellState = drawingSettings.BrushCellState
    };

    protected override void RestoreServiceSaveCore(TSave save)
    {
        drawingSettings.IsDrawingEnabled = save.IsDrawingEnabled;
        drawingSettings.BrushShape = save.DrawingBrushShape;
        drawingSettings.BrushRadius = save.DrawingBrushRadius;
        drawingSettings.BrushCellState = save.DrawingBrushCellState;
    }
}