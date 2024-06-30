using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.Presentation.StartupOperations;

public static class StartupOperationsDependencyInjection
{
    public static IServiceCollection AddStartupOperationManager(this IServiceCollection services) => services
        .AddSingleton<IStartupOperationManager, StartupOperationManager>();
    
    public static IServiceCollection AddStartupOperation<TOperation>
    (
        this IServiceCollection services
    )
        where TOperation : class, IStartupOperation
    {
        return services.AddSingleton<IStartupOperation, TOperation>();
    }
}