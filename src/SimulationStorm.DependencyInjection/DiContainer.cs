using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.DependencyInjection;

/// <summary>
/// Represents the dependency injection container with the ability to reuse a container instance
/// and the ability to pre-create registered singleton services during configuration.
/// </summary>
public class DiContainer : IDiContainer
{
    /// <summary>
    /// Gets the default <see cref="DiContainer"/> instance.
    /// </summary>
    public static DiContainer Default { get; } = new();

    /// <inheritdoc/>
    public bool IsConfigured => _serviceProvider is not null;

    #region Fields
    private ServiceProvider? _serviceProvider;
    
    private IReadOnlyCollection<Type>? _singletonServiceTypes;
    #endregion

    #region Public methods
    /// <inheritdoc cref="IDiContainer.GetService"/>
    public object? GetService(Type serviceType)
    {
        if (!IsConfigured)
            throw new InvalidOperationException("The container has not been configured.");
        
        return _serviceProvider!.GetService(serviceType);
    }
    
    /// <inheritdoc/>
    public void Configure(IServiceCollection services, bool activateSingletonServices = false)
    {
        if (IsConfigured)
            throw new InvalidOperationException("The container has already been configured.");
        
        _serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });

        _singletonServiceTypes = services
            .Where(serviceDescriptor =>
                serviceDescriptor.Lifetime is ServiceLifetime.Singleton
                && !serviceDescriptor.ServiceType.IsGenericTypeDefinition)
            .Select(serviceDescriptor => serviceDescriptor.ServiceType)
            .ToArray();

        if (activateSingletonServices)
            ActivateSingletonServices();
    }

    /// <inheritdoc cref="IDiContainer.Dispose"/>
    public void Dispose()
    {
        if (!IsConfigured)
            throw new InvalidOperationException("The container has not been configured.");
        
        _serviceProvider!.Dispose();
        
        _serviceProvider = null;
        _singletonServiceTypes = null;
        
        GC.SuppressFinalize(this);
    }
    
    /// <inheritdoc cref="IDiContainer.DisposeAsync"/>
    public async ValueTask DisposeAsync()
    {
        if (!IsConfigured)
            throw new InvalidOperationException("The container has not been configured.");

        await _serviceProvider!
            .DisposeAsync()
            .ConfigureAwait(false);

        _serviceProvider = null;
        _singletonServiceTypes = null;
        
        GC.SuppressFinalize(this);
    }
    #endregion

    private void ActivateSingletonServices() => _ = _singletonServiceTypes!
        .Select(serviceType => _serviceProvider!.GetService(serviceType)!)
        .Distinct()
        .ToArray();
}