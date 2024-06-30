using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimulationStorm.DependencyInjection;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Adds a shared singleton instance of the specified implementation type for interface type.
    /// </summary>
    /// <typeparam name="TService">The interface type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="services"/> parameter is <c>null</c>.</exception>
    public static IServiceCollection AddSharedSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class where TImplementation : class, TService =>
            services.AddSingleton<TService>(sp => sp.GetRequiredService<TImplementation>());
}