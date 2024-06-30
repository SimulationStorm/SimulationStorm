using System.Collections.Generic;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelStates
{
    public IDictionary<ToolPanel, bool> ToolPanelVisibilities { get; init; } = null!;
}