using System;

namespace SimulationStorm.Densities.Presentation;

/// <summary>
/// Provides a mechanism to change a user interface density.
/// </summary>
public interface IUiDensityManager
{
    /// <summary>
    /// Gets the current user interface density.
    /// </summary>
    UiDensity CurrentDensity { get; }

    /// <summary>
    /// Occurs when the user interface density changes.
    /// </summary>
    event EventHandler? DensityChanged; 

    /// <summary>
    /// Changes the current user interface density to <see cref="newDensity"/>.
    /// </summary>
    /// <param name="newDensity">The new user interface density.</param>
    void ChangeDensity(UiDensity newDensity);
}