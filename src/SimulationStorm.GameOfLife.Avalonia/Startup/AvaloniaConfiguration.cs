using System;
using System.Globalization;
using SimulationStorm.GameOfLife.Presentation.Startup;
using SimulationStorm.Localization.Avalonia;
using SimulationStorm.Notifications.Avalonia;
using TablerIcons;

namespace SimulationStorm.GameOfLife.Avalonia.Startup;

public static class AvaloniaConfiguration
{
    public const string ApplicationName = PresentationConfiguration.ApplicationName;
    
    public const Icons ApplicationIcon = Icons.IconGrain;
    
    public static readonly LocalizationOptions LocalizationOptions = new()
    {
        DefaultCulture = CultureInfo.GetCultureInfo("en-US"),
        SetInitialCultureToDefault = false,
        SupportedCultures = new []
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("ru-RU")
        },
        AssembliesContainingLocaleResources = new []
        {
            "SimulationStorm.Avalonia",
            "SimulationStorm.Themes.Avalonia",
            "SimulationStorm.Densities.Avalonia",
            "SimulationStorm.Localization.Avalonia",
            "SimulationStorm.AppStates.Avalonia",
            "SimulationStorm.Notifications.Avalonia",
            "SimulationStorm.ToolPanels.Avalonia",
            "SimulationStorm.Collections.Avalonia",
            
            "SimulationStorm.Simulation.Avalonia",
            "SimulationStorm.Simulation.Bounded.Avalonia",
            "SimulationStorm.Simulation.Cellular.Avalonia",
            "SimulationStorm.Simulation.CellularAutomation.Avalonia",
            "SimulationStorm.Simulation.Running.Avalonia",
            "SimulationStorm.Simulation.Resetting.Avalonia",
            "SimulationStorm.Simulation.History.Avalonia",
            "SimulationStorm.Simulation.Statistics.Avalonia",
            "SimulationStorm.GameOfLife.Avalonia"
        },
        LocaleResourcesDirectoryPath = "Resources/LocaleResources",
        StringResourceKeyPrefix = "Strings."
    };

    public static readonly NotificationsOptions NotificationsOptions = new()
    {
        DefaultExpirationTime = TimeSpan.FromSeconds(3),
        StringResourceKeyPrefix = "Strings."
    };
}