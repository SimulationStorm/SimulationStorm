using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelViewModelFactory
(
    IServiceProvider serviceProvider,
    ToolPanelOptions options
)
    : IToolPanelViewModelFactory
{
    public object Create(ToolPanel toolPanel)
    {
        if (!options.ToolPanelViewModelTypes.TryGetValue(toolPanel, out var viewModelType))
            throw new NotSupportedException($"A view model is not configured for tool panel {toolPanel.Name}");
        
        return serviceProvider.GetRequiredService(viewModelType);
    }
}