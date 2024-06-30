using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Presentation.Colors;

namespace SimulationStorm.Avalonia.Colors;

public static class UiColorsDependencyInjection
{
    public static IServiceCollection AddUiColorProvider(this IServiceCollection services) => services
        .AddSingleton<IUiColorProvider>(_ => new UiColorProvider(AvaloniaServiceProvider.GetApplicationOrThrow()));
}