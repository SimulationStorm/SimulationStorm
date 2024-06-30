using System;

using Avalonia;
using SimulationStorm.Avalonia;
using SimulationStorm.GameOfLife.Avalonia.Startup;

namespace SimulationStorm.GameOfLife.Avalonia.Desktop;

public sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        var appBuilder = BuildAvaloniaApp();
        var exitCode = appBuilder.StartWithClassicDesktopLifetime(args);
        ((SingleProcessApplication)appBuilder.Instance!).Dispose();
        return exitCode;
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect()
#if DEBUG
        .LogToTrace()
#endif
        ;
}