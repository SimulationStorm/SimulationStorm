using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.ToolPanels.Presentation;
using TablerIcons;

namespace SimulationStorm.ToolPanels.Avalonia;

public partial class ToolPanelMenuButton : Button
{
    public ToolPanelMenuButton()
    {
        InitializeComponent();

        var toolPanelManager = DiContainer.Default.GetRequiredService<IToolPanelManager>();

        var menuFlyout = new MenuFlyout();
        Flyout = menuFlyout;
        
        menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "theme-menuitem-heading" },
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable("Strings.ToolPanels.ToolPanels")
                .ToBinding()
        });

        toolPanelManager.ToolPanels.ForEach(toolPanel => menuFlyout.Items.Add(new MenuItem
        {
            Command = new RelayCommand(() => toolPanelManager.OpenToolPanel(toolPanel)),

            [!MenuItem.IconProperty] = this
                .GetResourceObservable($"Icons.ToolPanels.{toolPanel.Name}", obj =>
                {
                    if (obj is Icons icon)
                        return new TablerIcon { Icon = icon };
                            
                    return AvaloniaProperty.UnsetValue;
                })
                .ToBinding(),
                
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable($"Strings.ToolPanels.{toolPanel.Name}")
                .ToBinding(),
                
            [!MenuItem.InputGestureProperty] = this
                .GetResourceObservable($"KeyGestures.ToolPanels.{toolPanel.Name}")
                .ToBinding()
        }));
        
        IRelayCommand closeAllToolPanelsCommand = null!;
        closeAllToolPanelsCommand = new RelayCommand
        (
            () =>
            {
                toolPanelManager.CloseAllToolPanels();
                closeAllToolPanelsCommand.NotifyCanExecuteChanged();
            },
            () => toolPanelManager.ToolPanelVisibilities.Any(kv => kv.Value)
        );

        var closeAllToolPanelsMenuItem = new MenuItem
        {
            Command = closeAllToolPanelsCommand,
            
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable("Strings.ToolPanels.HideAllToolPanels")
                .ToBinding(),
            
            Icon = new TablerIcon { Icon = Icons.IconMinus }
        };
        closeAllToolPanelsMenuItem[!IsVisibleProperty] = closeAllToolPanelsMenuItem[!IsEffectivelyEnabledProperty];
        
        menuFlyout.Items.Add(new Separator
        {
            [!IsVisibleProperty] = closeAllToolPanelsMenuItem[!IsEffectivelyEnabledProperty]
        });
        menuFlyout.Items.Add(closeAllToolPanelsMenuItem);
    }
}