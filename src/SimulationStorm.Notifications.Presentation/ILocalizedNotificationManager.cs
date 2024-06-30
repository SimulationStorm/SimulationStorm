using System;

namespace SimulationStorm.Notifications.Presentation;

/// <summary>
/// Provides a mechanism to show localized notifications.
/// </summary>
public interface ILocalizedNotificationManager
{
    /// <summary>
    /// Shows a notification of the requested <see cref="type"/>.
    /// </summary>
    /// <param name="type">The notification type.</param>
    /// <param name="messageKey">The notification message key.</param>
    /// <param name="titleKey">The notification title key.</param>
    /// <param name="expiration">The expire time at which the notification will close.</param>
    /// <param name="onClick">An Action to call when the notification is clicked.</param>
    /// <param name="onClose">An Action to call when the notification is closed.</param>
    void ShowNotification(NotificationType type, string messageKey, string? titleKey = null,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null);
}