using System;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using NotificationType = SimulationStorm.Notifications.Presentation.NotificationType;

namespace SimulationStorm.Notifications.Avalonia;

public abstract class NotificationManagerBase(WindowNotificationManager notificationManager, NotificationsOptions options)
{
    protected void ShowNotificationCore(NotificationType type, string message, string? title,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (title is not null)
                ShowTitledNotification(type, message, title, expiration, onClick, onClose);
            else
                ShowUntitledNotification(type, message, expiration, onClick, onClose);
        });
    }

    #region Private methods
    private void ShowTitledNotification(NotificationType type, string message, string title,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.Show(new Notification(
            title, message, type.ToAvalonia(), options.DefaultExpirationTime, onClick, onClose));
    }
    
    private void ShowUntitledNotification(NotificationType type, string message,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        notificationManager.Show(message, type.ToAvalonia(), options.DefaultExpirationTime, onClick, onClose);
    }
    #endregion
}