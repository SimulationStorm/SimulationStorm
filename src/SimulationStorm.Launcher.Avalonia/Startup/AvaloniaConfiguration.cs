using System;
using System.Globalization;
using SimulationStorm.Localization.Avalonia;
using SimulationStorm.Notifications.Avalonia;

namespace SimulationStorm.Launcher.Avalonia.Startup;

public static class AvaloniaConfiguration
{
    public const string ApplicationName = "SimulationStorm | Launcher";
    
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
            "SimulationStorm.Notifications.Avalonia",
            "SimulationStorm.Launcher.Avalonia"
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