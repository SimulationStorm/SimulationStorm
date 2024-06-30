using System;

namespace SimulationStorm.Presentation;

/// <summary>
/// Provides a mechanism to shut down the application.
/// </summary>
public interface IShutdownService
{
    /// <summary>
    /// Gets whether the shutdown of the application is supported.
    /// </summary>
    bool IsShutdownSupported { get; }
    
    /// <summary>
    /// Gets whether the application can be shut down immediately.
    /// If immediate shutdown is not supported, then
    /// the application should release used resources
    /// via <see cref="IDisposable"/> and/or <see cref="IAsyncDisposable"/>.
    /// </summary>
    bool IsImmediateShutdownSupported { get; }

    /// <summary>
    /// Shutdowns the application.
    /// </summary>
    void Shutdown();
}