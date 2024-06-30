using System;

namespace SimulationStorm.Notifications.Presentation;

/// <summary>
/// Provides a mechanism to show notifications.
/// </summary>
public interface INotificationManager
{
    /// <summary>
    /// Shows a notification of the requested <see cref="type"/>.
    /// </summary>
    /// <param name="type">The notification type.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="title">The notification title.</param>
    /// <param name="expiration">The expire time at which the notification will close.</param>
    /// <param name="onClick">An Action to call when the notification is clicked.</param>
    /// <param name="onClose">An Action to call when the notification is closed.</param>
    void ShowNotification(NotificationType type, string message, string? title,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null);
}