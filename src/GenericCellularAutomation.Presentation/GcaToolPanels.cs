using SimulationStorm.ToolPanels.Presentation;

namespace GenericCellularAutomation.Presentation;

public static class GcaToolPanels
{
    public static readonly ToolPanel
        SimulationToolPanel = new(nameof(SimulationToolPanel)),
        DrawingToolPanel = new(nameof(DrawingToolPanel)),
        RenderingToolPanel = new(nameof(RenderingToolPanel)),
        CameraToolPanel = new(nameof(CameraToolPanel)),
        HistoryToolPanel = new(nameof(HistoryToolPanel)),
        StatisticsToolPanel = new(nameof(StatisticsToolPanel));
}