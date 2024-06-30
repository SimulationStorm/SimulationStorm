using System;

namespace SimulationStorm.Notifications.Presentation;

public static class LocalizedNotificationManagerExtensions
{
    public static void ShowInformation(this ILocalizedNotificationManager notificationManager, string messageKey,
        string? titleKey = null, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Information, messageKey, titleKey, expiration, onClick, onClose);
    }

    public static void ShowSuccess(this ILocalizedNotificationManager notificationManager, string messageKey,
        string? titleKey = null, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Success, messageKey, titleKey, expiration, onClick, onClose);
    }

    public static void ShowWarning(this ILocalizedNotificationManager notificationManager, string messageKey,
        string? titleKey = null, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Warning, messageKey, titleKey, expiration, onClick, onClose);
    }

    public static void ShowError(this ILocalizedNotificationManager notificationManager, string messageKey,
        string? titleKey = null, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.ShowNotification(NotificationType.Error, messageKey, titleKey, expiration, onClick, onClose);
    }
}