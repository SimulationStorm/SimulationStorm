using SimulationStorm.AppStates;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.Services;

public class DrawingSettingsStateManager<TCellState, TState>
(
    IDrawingSettings<TCellState> drawingSettings
)
    : ServiceStateManagerBase<TState>
    where TState : DrawingSettingsState<TCellState>, new()
{
    protected override TState SaveServiceStateImpl() => new()
    {
        IsDrawingEnabled = drawingSettings.IsDrawingEnabled,
        DrawingBrushShape = drawingSettings.BrushShape,
        DrawingBrushRadius = drawingSettings.BrushRadius,
        DrawingBrushCellState = drawingSettings.BrushCellState
    };

    protected override void RestoreServiceStateImpl(TState state)
    {
        drawingSettings.IsDrawingEnabled = state.IsDrawingEnabled;
        drawingSettings.BrushShape = state.DrawingBrushShape;
        drawingSettings.BrushRadius = state.DrawingBrushRadius;
        drawingSettings.BrushCellState = state.DrawingBrushCellState;
    }
}