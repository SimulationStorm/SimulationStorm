using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Themes.Presentation;

namespace SimulationStorm.Themes.Avalonia;

public partial class UiThemeMenuButton : Button
{
    public UiThemeMenuButton()
    {
        InitializeComponent();
        
        var uiThemeManager = DiContainer.Default.GetRequiredService<IUiThemeManager>();

        var menuFlyout = new MenuFlyout();
        Flyout = menuFlyout;

        menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "theme-menuitem-heading" },
            
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable("Strings.Themes.Theme")
                .ToBinding()
        });

        var uiThemes = Enum.GetValues<UiTheme>();
        uiThemes.ForEach(uiTheme => menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "has-check-box" },
            
            Header = uiTheme,
            IsChecked = uiTheme == uiThemeManager.CurrentTheme,
            Command = new RelayCommand(() => uiThemeManager.ChangeTheme(uiTheme)),
            
            [!HeaderedSelectingItemsControl.HeaderTemplateProperty] = this
                .GetResourceObservable("Templates.Themes.UiTheme")
                .ToBinding(),
                
            [!MenuItem.IsCheckedProperty] = Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => uiThemeManager.ThemeChanged += h,
                    h => uiThemeManager.ThemeChanged -= h
                )
                .Select(_ => uiThemeManager.CurrentTheme == uiTheme)
                .ToBinding()
        }));
    }
}