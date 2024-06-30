using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Operations;
using SimulationStorm.AppStates.Persistence;
using SimulationStorm.AppStates.Presentation.ViewModels;

namespace SimulationStorm.AppStates.Presentation;

public static class AppStatesDependencyInjection
{
    public static IServiceCollection AddAppStateManagementServices
    (
        this IServiceCollection services,
        AppStatesOptions options)
    {
        return services
            .AddSingleton(options)
            .AddSingleton<AppStatesDatabaseContext>()
            .AddSingleton<IEntityFactory, EntityFactory>()
            .AddSingleton<IAppStateRepository, AppStateRepository>()
            .AddSingleton<IAppStateManager, AppStateManager>()
            .AddSingleton<AppStateManagerDialogViewModel>()
            .AddTransient<SaveAppStateFlyoutViewModel>()
            .AddTransient<EditAppStateFlyoutViewModel>()
            .AddTransient<DeleteAllAppStatesFlyoutViewModel>();
    }

    public static IServiceCollection AddServiceStateManager<TServiceStateManager>
    (
        this IServiceCollection services
    )
        where TServiceStateManager : class, IServiceStateManager
    {
        return services.AddSingleton<IServiceStateManager, TServiceStateManager>();
    }
    
    public static IServiceCollection AddAsyncServiceStateManager<TAsyncServiceStateManager>
    (
        this IServiceCollection services
    )
        where TAsyncServiceStateManager : class, IAsyncServiceStateManager
    {
        return services.AddSingleton<IAsyncServiceStateManager, TAsyncServiceStateManager>();
    }

    public static IServiceCollection AddAppStateRestoringOperation<TOperation>
    (
        this IServiceCollection services
    )
        where TOperation : class, IAppStateRestoringOperation
    {
        return services.AddSingleton<IAppStateRestoringOperation, TOperation>();
    }
}