using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.LiveChartsExtensions.Avalonia;

public static class LvcDependencyInjection
{
    public static IServiceCollection AddLiveCharts(this IServiceCollection services) => services
        .AddSingleton(LvcConfiguration.LvcOptions)
        .AddSingleton<LvcConfigurator>()
        .AddSingleton<LvcThemeUpdater>();
}