using System;

namespace SimulationStorm.Presentation;

/// <summary>
/// Provides a mechanism to shut down the application.
/// </summary>
/// <param name="isImmediateShutdownSupported">Is application can be shut down immediately.</param>
/// <param name="shutdownAction">The action to shut down the application, or null if application shutting down is not supported.</param>
public class ShutdownService(Action? shutdownAction, bool isImmediateShutdownSupported = false) : IShutdownService
{
    /// <inheritdoc/>
    public bool IsShutdownSupported => shutdownAction is not null;

    /// <inheritdoc/>
    public bool IsImmediateShutdownSupported => isImmediateShutdownSupported;

    /// <inheritdoc/>
    public void Shutdown()
    {
        if (shutdownAction is null)
            throw new NotSupportedException();
        
        shutdownAction();
    }
}