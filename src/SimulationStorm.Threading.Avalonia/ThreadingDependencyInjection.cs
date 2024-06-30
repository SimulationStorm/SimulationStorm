using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Threading.Avalonia;

public static class ThreadingDependencyInjection
{
    public static IServiceCollection AddThreadingServices(this IServiceCollection services) => services
        .AddUiThreadScheduler()
        .AddImmediateUiThreadScheduler();
    
    public static IServiceCollection AddUiThreadScheduler(this IServiceCollection services) =>
        services.AddSingleton<IUiThreadScheduler, UiThreadScheduler>();

    public static IServiceCollection AddImmediateUiThreadScheduler(this IServiceCollection services) =>
        services.AddSingleton<IImmediateUiThreadScheduler, ImmediateUiThreadScheduler>();
}