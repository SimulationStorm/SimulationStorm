using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Operations;
using SimulationStorm.AppSaves.Persistence;
using SimulationStorm.AppSaves.Presentation.ViewModels;

namespace SimulationStorm.AppSaves.Presentation;

public static class AppSavesDependencyInjection
{
    public static IServiceCollection AddAppSaveManagementServices
    (
        this IServiceCollection services,
        AppSavesOptions options)
    {
        return services
            .AddSingleton(options)
            .AddSingleton<AppSavesDatabaseContext>()
            .AddSingleton<IAppSavesEntityFactory, AppSavesEntityFactory>()
            .AddSingleton<IAppSaveRepository, AppSaveRepository>()
            .AddSingleton<IAppSaveManager, AppSaveManager>()
            .AddSingleton<AppSaveManagerDialogViewModel>()
            .AddTransient<SaveAppFlyoutViewModel>()
            .AddTransient<EditAppSaveFlyoutViewModel>()
            .AddTransient<DeleteAllAppSavesFlyoutViewModel>();
    }

    public static IServiceCollection AddServiceSaveManager<TServiceSaveManager>
    (
        this IServiceCollection services
    )
        where TServiceSaveManager : class, IServiceSaveManager
    {
        return services.AddSingleton<IServiceSaveManager, TServiceSaveManager>();
    }
    
    public static IServiceCollection AddAsyncServiceSaveManager<TAsyncServiceSaveManager>
    (
        this IServiceCollection services
    )
        where TAsyncServiceSaveManager : class, IAsyncServiceSaveManager
    {
        return services.AddSingleton<IAsyncServiceSaveManager, TAsyncServiceSaveManager>();
    }

    public static IServiceCollection AddAppSaveRestoringOperation<TOperation>
    (
        this IServiceCollection services
    )
        where TOperation : class, IAppSaveRestoringOperation
    {
        return services.AddSingleton<IAppSaveRestoringOperation, TOperation>();
    }
    
    public static IServiceCollection AddAppSaveRestoredOperation<TOperation>
    (
        this IServiceCollection services
    )
        where TOperation : class, IAppSaveRestoredOperation
    {
        return services.AddSingleton<IAppSaveRestoredOperation, TOperation>();
    }
}