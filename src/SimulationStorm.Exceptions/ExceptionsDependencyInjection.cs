using System;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Exceptions.FirstChance;
using SimulationStorm.Exceptions.Notifiers;
using SimulationStorm.Exceptions.Unhandled;

namespace SimulationStorm.Exceptions;

public static class ExceptionsDependencyInjection
{
    public static IServiceCollection AddAppDomainExceptionNotifier
    (
        this IServiceCollection services,
        AppDomain? appDomain = null)
    {
        return services
            .AddSingleton(_ => new AppDomainExceptionNotifier(appDomain ?? AppDomain.CurrentDomain))
            .AddSingleton<INotifyFirstChanceException>(sp => sp.GetRequiredService<AppDomainExceptionNotifier>())
            .AddSingleton<INotifyUnhandledException>(sp => sp.GetRequiredService<AppDomainExceptionNotifier>());
    }

    public static IServiceCollection AddTaskSchedulerExceptionNotifier(this IServiceCollection services) =>
        services.AddSingleton<INotifyUnhandledException, TaskSchedulerExceptionNotifier>();
    
    public static IServiceCollection AddTaskSchedulerExceptionBubbler(this IServiceCollection services) =>
        services.AddSingleton<TaskSchedulerExceptionBubbler>();

    public static IServiceCollection AddExceptionHandlersToNotifiersSubscriber(this IServiceCollection services) =>
        services.AddSingleton<ExceptionHandlersToNotifiersSubscriber>();

    public static IServiceCollection AddUnhandledExceptionSuppressor(this IServiceCollection services) =>
        services.AddSingleton<UnhandledExceptionSuppressor>();
}