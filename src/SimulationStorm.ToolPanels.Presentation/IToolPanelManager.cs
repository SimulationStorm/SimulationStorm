using System;
using System.Collections.Generic;

namespace SimulationStorm.ToolPanels.Presentation;

// Todo: Could be refactored
public interface IToolPanelManager
{
    #region Properties
    IEnumerable<ToolPanel> ToolPanels { get; }
    
    IReadOnlyDictionary<ToolPanel, bool> ToolPanelVisibilities { get; }
    
    IReadOnlyDictionary<ToolPanel, ToolPanelPosition> ToolPanelPositions { get; }
    #endregion
    
    event EventHandler<ToolPanelAtPositionChangedEventArgs>? ToolPanelAtPositionChanged;

    #region Methods
    void OpenToolPanel(ToolPanel toolPanel);

    void CloseToolPanel(ToolPanel toolPanel);

    void CloseAllToolPanels();

    void ToggleToolPanelVisibility(ToolPanel toolPanel);

    ToolPanel? GetOpenedToolPanelAtPosition(ToolPanelPosition position);
    #endregion
}