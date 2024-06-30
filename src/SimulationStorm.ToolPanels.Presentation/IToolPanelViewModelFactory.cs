namespace SimulationStorm.ToolPanels.Presentation;

public interface IToolPanelViewModelFactory
{
    object Create(ToolPanel toolPanel);
}