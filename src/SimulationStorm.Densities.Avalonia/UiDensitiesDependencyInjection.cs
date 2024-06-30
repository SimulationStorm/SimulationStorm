using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Presentation;
using SimulationStorm.Avalonia;
using SimulationStorm.Densities.Presentation;

namespace SimulationStorm.Densities.Avalonia;

public static class UiDensitiesDependencyInjection
{
    public static IServiceCollection AddUiDensityManager(this IServiceCollection services) => services
        .AddSingleton<IUiDensityManager>(_ => new UiDensityManager(AvaloniaServiceProvider.GetModernThemeOrThrow()))
        .AddServiceStateManager<UiDensityStateManager>();
}