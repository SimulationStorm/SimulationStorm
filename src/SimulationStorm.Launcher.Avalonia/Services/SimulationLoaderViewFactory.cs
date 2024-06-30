using System.ComponentModel;
using SimulationStorm.Launcher.Presentation.Models;
using SimulationStorm.Launcher.Presentation.Services;
using SimulationStorm.GameOfLife.Avalonia.Startup;
using SimulationStorm.Presentation;

namespace SimulationStorm.Launcher.Avalonia.Services;

/// <inheritdoc/>
public class SimulationLoaderViewFactory
(
    IShutdownService shutdownService
)
    : ISimulationLoaderViewFactory
{
    /// <inheritdoc/>
    public object Create(SimulationType simulationType) => simulationType switch
    {
        SimulationType.GameOfLife => new LoaderView(shutdownService),
        _ => throw new InvalidEnumArgumentException(nameof(simulationType), (int)simulationType, typeof(SimulationType))
    };
}