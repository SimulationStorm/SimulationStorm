using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation;
using SimulationStorm.Avalonia;
using SimulationStorm.Densities.Presentation;

namespace SimulationStorm.Densities.Avalonia;

public static class UiDensitiesDependencyInjection
{
    public static IServiceCollection AddUiDensityManager(this IServiceCollection services) => services
        .AddSingleton<IUiDensityManager>(_ => new UiDensityManager(AvaloniaServiceProvider.GetModernThemeOrThrow()))
        .AddServiceSaveManager<UiDensitySaveManager>();
}