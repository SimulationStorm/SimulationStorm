using System;

namespace SimulationStorm.Notifications.Presentation;

public static class NotificationManagerExtensions
{
    public static void ShowInformation(this INotificationManager notificationManager, string message, string? title = null,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Information, message, title, expiration, onClick, onClose);
    }

    public static void ShowSuccess(this INotificationManager notificationManager, string message, string? title = null,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Success, message, title, expiration, onClick, onClose);
    }

    public static void ShowWarning(this INotificationManager notificationManager, string message, string? title = null,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Warning, message, title, expiration, onClick, onClose);
    }

    public static void ShowError(this INotificationManager notificationManager, string message, string? title = null,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Error, message, title, expiration, onClick, onClose);
    }
}