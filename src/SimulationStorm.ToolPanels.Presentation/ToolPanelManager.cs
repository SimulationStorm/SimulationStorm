using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationStorm.ToolPanels.Presentation;

public class ToolPanelManager(ToolPanelOptions options) : IToolPanelManager
{
    #region Properties
    public IEnumerable<ToolPanel> ToolPanels => _toolPanelVisibilities.Keys;
    
    public IReadOnlyDictionary<ToolPanel, bool> ToolPanelVisibilities =>
        (IReadOnlyDictionary<ToolPanel, bool>)_toolPanelVisibilities;

    public IReadOnlyDictionary<ToolPanel, ToolPanelPosition> ToolPanelPositions { get; } = options.ToolPanelPositions;
    #endregion
    
    public event EventHandler<ToolPanelAtPositionChangedEventArgs>? ToolPanelAtPositionChanged;

    private readonly IDictionary<ToolPanel, bool> _toolPanelVisibilities =
        new Dictionary<ToolPanel, bool>(options.ToolPanelVisibilities);

    #region Methods
    public void OpenToolPanel(ToolPanel toolPanel)
    {
        ValidateToolPanel(toolPanel);
        
        if (!_toolPanelVisibilities[toolPanel])
            ToggleToolPanelVisibility(toolPanel);
    }

    public void CloseToolPanel(ToolPanel toolPanel)
    {
        ValidateToolPanel(toolPanel);
        
        if (_toolPanelVisibilities[toolPanel])
            ToggleToolPanelVisibility(toolPanel);
    }

    public void CloseAllToolPanels() => _toolPanelVisibilities
        .Where(kv => kv.Value)
        .Select(kv => kv.Key)
        .ToList()
        .ForEach(CloseToolPanel);

    public void ToggleToolPanelVisibility(ToolPanel toolPanel)
    {
        ValidateToolPanel(toolPanel);
        
        var position = ToolPanelPositions[toolPanel];
        var previousVisibleToolPanel = GetOpenedToolPanelAtPosition(position);

        ToolPanel? newVisibleToolPanel = null;
        
        if (previousVisibleToolPanel == toolPanel)
            _toolPanelVisibilities[toolPanel] = false;
        else
        {
            if (previousVisibleToolPanel is not null)
                _toolPanelVisibilities[previousVisibleToolPanel] = false;
            
            _toolPanelVisibilities[toolPanel] = true;
            newVisibleToolPanel = toolPanel;
        }

        ToolPanelAtPositionChanged?.Invoke(this,
            new ToolPanelAtPositionChangedEventArgs(position, previousVisibleToolPanel, newVisibleToolPanel));
    }

    public ToolPanel? GetOpenedToolPanelAtPosition(ToolPanelPosition position)
    {
        var panelsAtPosition = ToolPanelPositions.Where(x => x.Value == position).Select(x => x.Key);
        return _toolPanelVisibilities.FirstOrDefault(x => panelsAtPosition.Contains(x.Key) && x.Value).Key;
    }
    #endregion

    private void ValidateToolPanel(ToolPanel toolPanel)
    {
        if (!ToolPanelPositions.Keys.Contains(toolPanel))
            throw new InvalidOperationException($"The tool panel {toolPanel.Name} was not provided via options.");
    }
}