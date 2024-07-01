using System.Collections.Generic;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelsSave
{
    public IDictionary<ToolPanel, bool> ToolPanelVisibilities { get; init; } = null!;
}