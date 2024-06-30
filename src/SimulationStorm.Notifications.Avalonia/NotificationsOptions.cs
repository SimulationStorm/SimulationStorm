using System;

namespace SimulationStorm.Notifications.Avalonia;

public class NotificationsOptions
{
    public TimeSpan DefaultExpirationTime { get; init; }

    public string StringResourceKeyPrefix { get; init; } = string.Empty;
}