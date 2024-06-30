using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Densities.Avalonia;
using SimulationStorm.Launcher.Avalonia.Services;
using SimulationStorm.Launcher.Presentation.Services;
using SimulationStorm.Localization.Avalonia;
using SimulationStorm.Notifications.Avalonia;
using SimulationStorm.Themes.Avalonia;

namespace SimulationStorm.Launcher.Avalonia.Startup;

public static class AvaloniaDependencyInjection
{
    public static IServiceCollection AddAvaloniaServices
    (
        this IServiceCollection services,
        IResourceDictionary localeResourceHost)
    {
        return services
            .AddLocalizationManager(localeResourceHost, AvaloniaConfiguration.LocalizationOptions)
            .AddUiThemeManager()
            .AddUiDensityManager()
            .AddSingleton<ISimulationLoaderViewFactory, SimulationLoaderViewFactory>()
            .AddSingleton<ISimulationApplicationNameProvider, SimulationApplicationNameProvider>();
    }
}