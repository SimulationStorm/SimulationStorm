using System;
using Avalonia.Controls.ApplicationLifetimes;
using SimulationStorm.Presentation;

namespace SimulationStorm.Avalonia;

public abstract class SingleProcessApplication : DesktopApplication, IDisposable
{
    private const string DefaultApplicationName = "Avalonia Application";
    
    private IDisposable? _applicationProcessLock;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new InvalidOperationException($"A {nameof(SingleProcessApplication)} " +
                                                $"can only be run with a desktop lifetime.");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException($"The {nameof(SingleProcessApplication)} must have a name.");
        
        if (Name == DefaultApplicationName)
            throw new InvalidOperationException($"The name of the {nameof(SingleProcessApplication)} must be " +
                                                $"different from the default one (default is '{DefaultApplicationName}').");
            
        if (ApplicationProcessManager.TryGetLock(Name!, out _applicationProcessLock))
            InitializeApplication(desktop);
        else
            OnApplicationIsAlreadyRunning(desktop);
    }

    protected abstract void OnApplicationIsAlreadyRunning(IClassicDesktopStyleApplicationLifetime desktop);

    public void Dispose()
    {
        _applicationProcessLock?.Dispose();
        GC.SuppressFinalize(this);
    }
}