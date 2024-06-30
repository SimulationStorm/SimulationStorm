using System;
using System.Collections.Generic;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelOptions
{
    public IReadOnlyDictionary<ToolPanel, ToolPanelPosition> ToolPanelPositions { get; init; } = null!;

    public IReadOnlyDictionary<ToolPanel, bool> ToolPanelVisibilities { get; init; } = null!;

    public IReadOnlyDictionary<ToolPanel, Type> ToolPanelViewModelTypes { get; init; } = null!;
}