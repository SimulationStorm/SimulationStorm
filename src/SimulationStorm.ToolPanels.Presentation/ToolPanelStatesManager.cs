using System.Linq;
using SimulationStorm.AppSaves;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelSavesManager(IToolPanelManager toolPanelManager) : ServiceSaveManagerBase<ToolPanelsSave>
{
    protected override ToolPanelsSave SaveServiceCore() => new()
    {
        ToolPanelVisibilities = toolPanelManager.ToolPanelVisibilities.ToDictionary()
    };

    protected override void RestoreServiceSaveCore(ToolPanelsSave save)
    {
        foreach (var (toolPanel, isVisible) in save.ToolPanelVisibilities)
        {
            if (isVisible)
                toolPanelManager.OpenToolPanel(toolPanel);
            else
                toolPanelManager.CloseToolPanel(toolPanel);
        }
    }
}