namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelAtPositionChangedEventArgs
(
    ToolPanelPosition position,
    ToolPanel? previousVisibleToolPanel,
    ToolPanel? newVisibleToolPanel)
{
    public ToolPanelPosition Position { get; } = position;

    public ToolPanel? PreviousVisibleToolPanel { get; } = previousVisibleToolPanel;

    public ToolPanel? NewVisibleToolPanel { get; } = newVisibleToolPanel;
}