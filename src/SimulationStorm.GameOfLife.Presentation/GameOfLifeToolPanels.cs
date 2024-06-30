using SimulationStorm.ToolPanels.Presentation;

namespace SimulationStorm.GameOfLife.Presentation;

public static class GameOfLifeToolPanels
{
    public static readonly ToolPanel
        SimulationToolPanel = new(nameof(SimulationToolPanel)),
        DrawingToolPanel = new(nameof(DrawingToolPanel)),
        RenderingToolPanel = new(nameof(RenderingToolPanel)),
        CameraToolPanel = new(nameof(CameraToolPanel)),
        HistoryToolPanel = new(nameof(HistoryToolPanel)),
        StatisticsToolPanel = new(nameof(StatisticsToolPanel));
}