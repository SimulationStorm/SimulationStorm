using System;
using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;

namespace SimulationStorm.Avalonia;

public static class AvaloniaServiceProvider
{
    public static WindowNotificationManager GetNotificationManager() =>
        Dispatcher.UIThread.Invoke(() => new WindowNotificationManager(GetTopLevelOrThrow()));

    public static TopLevel GetTopLevelOrThrow() => GetTopLevel()
        ?? throw new InvalidOperationException($"The {nameof(TopLevel)} instance must exist.");

    public static TopLevel? GetTopLevel()
    {
        var topLevel = Dispatcher.UIThread.Invoke(() =>
        {
            var application = GetApplication();
            if (application is null)
                return null;

            var applicationVisualRoot = application.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
                ISingleViewApplicationLifetime singleView => singleView.MainView,
                _ => null
            };

            return TopLevel.GetTopLevel(applicationVisualRoot);
        });
        
        return topLevel;
    }

    public static Application GetApplicationOrThrow() => GetApplication()
        ?? throw new InvalidOperationException($"The {nameof(Application)} instance must exist.");

    public static Application? GetApplication() => Dispatcher.UIThread.Invoke(() => Application.Current);

    public static ModernTheme GetModernThemeOrThrow() => GetModernTheme()
        ?? throw new InvalidOperationException($"The {nameof(ModernTheme)} instance must exist.");

    public static ModernTheme? GetModernTheme()
    {
        ModernTheme? modernTheme = null;
        Dispatcher.UIThread.Invoke(() => ModernTheme.TryGetCurrent(out modernTheme));
        return modernTheme;
    }
}