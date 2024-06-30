using SimulationStorm.Launcher.Presentation.Models;

namespace SimulationStorm.Launcher.Presentation.Services;

/// <summary>
/// Represents a factory for creating loader views for different simulation types.
/// </summary>
public interface ISimulationLoaderViewFactory
{
    /// <summary>
    /// Creates a loader view for the specified simulation type.
    /// </summary>
    /// <param name="simulationType">The type of simulation for which to create the main view.</param>
    /// <returns>The created simulation loader view.</returns>
    object Create(SimulationType simulationType);
}