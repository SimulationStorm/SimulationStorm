using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace SimulationStorm.Avalonia;

public abstract class DesktopApplication : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new InvalidOperationException($"A {nameof(DesktopApplication)} can only be run with a desktop lifetime.");
        
        InitializeApplication(desktop);
    }

    protected abstract void InitializeApplication(IClassicDesktopStyleApplicationLifetime desktop);
}