using System;
using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Notifications.Presentation;
using AvaloniaNotificationManager = Avalonia.Controls.Notifications.WindowNotificationManager;

namespace SimulationStorm.Notifications.Avalonia;

/// <inheritdoc cref="INotificationManager"/>
public class LocalizedNotificationManager
(
    AvaloniaNotificationManager notificationManager,
    IResourceNode localeResourceNode,
    NotificationsOptions options
)
    : NotificationManagerBase(notificationManager, options), ILocalizedNotificationManager
{
    private readonly string _stringResourceKeyPrefix = options.StringResourceKeyPrefix;
    
    /// <inheritdoc/>
    public void ShowNotification(NotificationType type, string messageKey, string? titleKey,
        TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        var message = GetLocalizedString(messageKey);
        var title = titleKey is null ? null : GetLocalizedString(titleKey);
        
        ShowNotificationCore(type, message, title, expiration, onClick, onClose);
    }

    private string GetLocalizedString(string key)
    {
        key = $"{_stringResourceKeyPrefix}{key}";
        
        if (!localeResourceNode.TryGetResourceOnUiThread(key, out var resource))
            throw new InvalidOperationException($"The localized string with the key '{key}' was not found.");
        
        return (resource as string)!;
    }
}