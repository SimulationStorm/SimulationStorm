using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using SimulationStorm.ToolPanels.Presentation;

namespace SimulationStorm.ToolPanels.Avalonia;

public partial class ToolPanelButton : UserControl
{
    #region Avalonia properties
    public static readonly StyledProperty<ToolPanel?> ToolPanelProperty =
        AvaloniaProperty.Register<ToolPanelButton, ToolPanel?>(nameof(ToolPanel));

    public static readonly StyledProperty<ICommand?> ToggleCommandProperty =
        AvaloniaProperty.Register<ToolPanelButton, ICommand?>(nameof(ToggleCommand));

    public static readonly StyledProperty<IEnumerable<ToolPanel>?> OpenedToolPanelsProperty =
        AvaloniaProperty.Register<ToolPanelButton, IEnumerable<ToolPanel>?>(nameof(OpenedToolPanels));
    #endregion

    #region Properties
    public ToolPanel? ToolPanel
    {
        get => GetValue(ToolPanelProperty);
        set => SetValue(ToolPanelProperty, value);
    }

    public ICommand? ToggleCommand
    {
        get => GetValue(ToggleCommandProperty);
        set => SetValue(ToggleCommandProperty, value);
    }

    public IEnumerable<ToolPanel>? OpenedToolPanels
    {
        get => GetValue(OpenedToolPanelsProperty);
        set => SetValue(OpenedToolPanelsProperty, value);
    }
    #endregion
    
    public ToolPanelButton()
    {
        InitializeComponent();
        
        UpdateToggleButtonIsChecked();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property == OpenedToolPanelsProperty)
            UpdateToggleButtonIsChecked();
    }

    private void UpdateToggleButtonIsChecked() =>
        ToggleButton.IsChecked = OpenedToolPanels?.Contains(ToolPanel) is true;
}