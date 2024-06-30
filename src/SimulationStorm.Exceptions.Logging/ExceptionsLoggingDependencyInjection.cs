using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Exceptions.Unhandled;

namespace SimulationStorm.Exceptions.Logging;

public static class ExceptionsLoggingDependencyInjection
{
    public static IServiceCollection AddUnhandledExceptionLogger(this IServiceCollection services) =>
        services.AddSingleton<IHandleUnhandledException, UnhandledExceptionLogger>();
}