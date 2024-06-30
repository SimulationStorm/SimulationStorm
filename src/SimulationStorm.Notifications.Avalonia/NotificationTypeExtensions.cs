using System;
using SimulationStorm.Notifications.Presentation;
using AvaloniaNotificationType = Avalonia.Controls.Notifications.NotificationType;

namespace SimulationStorm.Notifications.Avalonia;

public static class NotificationTypeExtensions
{
    public static AvaloniaNotificationType ToAvalonia(this NotificationType notificationType) =>
        notificationType switch
    {
        NotificationType.Information => AvaloniaNotificationType.Information,
        NotificationType.Success => AvaloniaNotificationType.Success,
        NotificationType.Warning => AvaloniaNotificationType.Warning,
        NotificationType.Error => AvaloniaNotificationType.Error,
        _ => throw new ArgumentException("Invalid notification type.", nameof(notificationType))
    };

    public static NotificationType ToNotificationType(this AvaloniaNotificationType avaloniaNotificationType) =>
        avaloniaNotificationType switch
    {
        AvaloniaNotificationType.Information => NotificationType.Information,
        AvaloniaNotificationType.Success => NotificationType.Success,
        AvaloniaNotificationType.Warning => NotificationType.Warning,
        AvaloniaNotificationType.Error => NotificationType.Error,
        _ => throw new ArgumentException("Invalid notification type.", nameof(avaloniaNotificationType))
    };
}