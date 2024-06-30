using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.DependencyInjection;

/// <summary>
/// Represents the dependency injection container with the ability to reuse a container instance
/// and the ability to pre-create registered singleton services during configuration.
/// </summary>
public interface IDiContainer : IServiceProvider, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets whether the dependency injection container is configured.
    /// </summary>
    bool IsConfigured { get; }
    
    #region Methods
    /// <inheritdoc cref="IServiceProvider.GetService"/>
    /// <exception cref="InvalidOperationException">Thrown when the container has not been not configured.</exception>
    new object? GetService(Type serviceType);
    
    /// <summary>
    /// Configures the dependency injection container with the given service collection.
    /// </summary>
    /// <param name="services">The collection of services.</param>
    /// <param name="activateSingletonServices">Whether it is needed to pre-create registered singleton services.</param>
    /// <exception cref="InvalidOperationException">Thrown when the container has already been configured.</exception>
    void Configure(IServiceCollection services, bool activateSingletonServices = false);

    /// <summary>
    /// Disposes the registered singleton services which implement <see cref="IDisposable"/>.
    /// After disposing, the container can be configured again.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the container is not configured.</exception>
    new void Dispose();
    
    /// <summary>
    /// Disposes the registered singleton services which implement <see cref="IAsyncDisposable"/> and <see cref="IDisposable"/> asynchronously.
    /// After disposing, the container can be configured again.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the container is not configured.</exception>
    new ValueTask DisposeAsync();
    #endregion
}