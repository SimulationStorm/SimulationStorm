using System;
using SimulationStorm.Notifications.Presentation;
using AvaloniaNotificationManager = Avalonia.Controls.Notifications.WindowNotificationManager;

namespace SimulationStorm.Notifications.Avalonia;

/// <inheritdoc cref="INotificationManager"/>
public class NotificationManager
(
    AvaloniaNotificationManager notificationManager,
    NotificationsOptions options
)
    : NotificationManagerBase(notificationManager, options), INotificationManager
{
    /// <inheritdoc/>
    public void ShowNotification(NotificationType type, string message, string? title,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        ShowNotificationCore(type, message, title, expiration, onClick, onClose);
    }
}