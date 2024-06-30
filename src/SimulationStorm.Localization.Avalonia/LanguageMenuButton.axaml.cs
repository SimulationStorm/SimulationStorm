using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using DotNext.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Localization.Presentation;

namespace SimulationStorm.Localization.Avalonia;

public partial class LanguageMenuButton : Button
{
    public LanguageMenuButton()
    {
        InitializeComponent();
        
        var localizationManager = DiContainer.Default.GetRequiredService<ILocalizationManager>();
        
        var menuFlyout = new MenuFlyout();
        Flyout = menuFlyout;

        menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "theme-menuitem-heading" },
            
            [!HeaderedSelectingItemsControl.HeaderProperty] = this
                .GetResourceObservable("Strings.Localization.Language")
                .ToBinding()
        });
        
        localizationManager.SupportedCultures.ForEach(culture => menuFlyout.Items.Add(new MenuItem
        {
            Classes = { "has-check-box" },
            Header = culture.NativeName,
            IsChecked = culture.Equals(localizationManager.CurrentCulture),
            Command = new RelayCommand(() => localizationManager.ChangeCulture(culture)),
            
            [!MenuItem.IsCheckedProperty] = Observable
                .FromEventPattern<EventHandler<CultureChangedEventArgs>, CultureChangedEventArgs>
                (
                    h => localizationManager.CultureChanged += h,
                    h => localizationManager.CultureChanged -= h
                )
                .Select(e => e.EventArgs.NewCulture.Equals(culture))
                .ToBinding()
        }));
    }
}