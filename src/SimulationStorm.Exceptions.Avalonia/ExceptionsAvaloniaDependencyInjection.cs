using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Exceptions.FirstChance;
using SimulationStorm.Exceptions.Unhandled;

namespace SimulationStorm.Exceptions.Avalonia;

public static class ExceptionsAvaloniaDependencyInjection
{
    public static IServiceCollection AddDispatcherExceptionNotifier
    (
        this IServiceCollection services,
        Dispatcher? dispatcher = null)
    {
        return services
            .AddSingleton(_ => new DispatcherExceptionNotifier(dispatcher ?? Dispatcher.UIThread))
            .AddSingleton<INotifyFirstChanceException>(sp => sp.GetRequiredService<DispatcherExceptionNotifier>())
            .AddSingleton<INotifyUnhandledException>(sp => sp.GetRequiredService<DispatcherExceptionNotifier>());
    }
}