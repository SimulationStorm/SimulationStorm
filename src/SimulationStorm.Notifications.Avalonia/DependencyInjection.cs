using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia;
using SimulationStorm.Notifications.Presentation;
using INotificationManager = SimulationStorm.Notifications.Presentation.INotificationManager;

namespace SimulationStorm.Notifications.Avalonia;

public static class NotificationsDependencyInjection
{
    public static IServiceCollection AddNotificationManagers
    (
        this IServiceCollection services,
        IResourceNode localeResourceNode,
        NotificationsOptions options)
    {
        return services
            .AddNotificationManager(options)
            .AddLocalizedNotificationManager(localeResourceNode, options);
    }

    public static IServiceCollection AddNotificationManager
    (
        this IServiceCollection services,
        NotificationsOptions options)
    {
        return services
            .AddSingleton<INotificationManager>(_ =>
                new NotificationManager(AvaloniaServiceProvider.GetNotificationManager(), options));
    }
    
    public static IServiceCollection AddLocalizedNotificationManager
    (
        this IServiceCollection services,
        IResourceNode localeResourceNode,
        NotificationsOptions options)
    {
        return services.AddSingleton<ILocalizedNotificationManager>(_ => new LocalizedNotificationManager(
            AvaloniaServiceProvider.GetNotificationManager(), localeResourceNode, options));
    }
    
    public static IServiceCollection AddNotificationManagers
    (
        this IServiceCollection services,
        WindowNotificationManager windowNotificationManager,
        IResourceNode localeResourceNode,
        NotificationsOptions options)
    {
        return services
            .AddNotificationManager(windowNotificationManager, options)
            .AddLocalizedNotificationManager(windowNotificationManager, localeResourceNode, options);
    }
    
    public static IServiceCollection AddNotificationManager
    (
        this IServiceCollection services,
        WindowNotificationManager windowNotificationManager,
        NotificationsOptions options)
    {
        return services
            .AddSingleton<INotificationManager>(_ => new NotificationManager(windowNotificationManager, options));
    }
    
    public static IServiceCollection AddLocalizedNotificationManager
    (
        this IServiceCollection services,
        WindowNotificationManager windowNotificationManager,
        IResourceNode localeResourceNode,
        NotificationsOptions options)
    {
        return services.AddSingleton<ILocalizedNotificationManager>(_ => new LocalizedNotificationManager(
            windowNotificationManager, localeResourceNode, options));
    }
}