using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Densities.Presentation;
using SimulationStorm.DependencyInjection;

namespace SimulationStorm.Densities.Avalonia;

public partial class UiDensityMenuButton : Button
{
    public UiDensityMenuButton()
    {
        InitializeComponent();
        
        var uiDensityManager = DiContainer.Default.GetRequiredService<IUiDensityManager>();

        var menuFlyout = new MenuFlyout();
        Flyout = menuFlyout;

        menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "theme-menuitem-heading" },
            
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable("Strings.Densities.Density")
                .ToBinding()
        });

        var uiDensities = Enum.GetValues<UiDensity>();
        uiDensities.ForEach(uiDensity =>
        {
            menuFlyout.Items.Add(new MenuItem
            {
                Classes = { "has-check-box" },
                IsChecked = uiDensity == uiDensityManager.CurrentDensity,
                Command = new RelayCommand(() => uiDensityManager.ChangeDensity(uiDensity)),
                
                [!HeaderedSelectingItemsControl.HeaderProperty] = this
                    .GetResourceObservable($"Strings.Densities.{uiDensity}")
                    .ToBinding(),
                
                [!MenuItem.IsCheckedProperty] = Observable
                    .FromEventPattern<EventHandler, EventArgs>
                    (
                        h => uiDensityManager.DensityChanged += h,
                        h => uiDensityManager.DensityChanged -= h
                    )
                    .Select(e => uiDensityManager.CurrentDensity == uiDensity)
                    .ToBinding()
            });
        });
    }
}