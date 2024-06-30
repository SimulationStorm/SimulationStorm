using System.Linq;
using SimulationStorm.AppStates;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelStatesManager(IToolPanelManager toolPanelManager) : ServiceStateManagerBase<ToolPanelStates>
{
    protected override ToolPanelStates SaveServiceStateImpl() => new()
    {
        ToolPanelVisibilities = toolPanelManager.ToolPanelVisibilities.ToDictionary()
    };

    protected override void RestoreServiceStateImpl(ToolPanelStates state)
    {
        foreach (var (toolPanel, isVisible) in state.ToolPanelVisibilities)
        {
            if (isVisible)
                toolPanelManager.OpenToolPanel(toolPanel);
            else
                toolPanelManager.CloseToolPanel(toolPanel);
        }
    }
}