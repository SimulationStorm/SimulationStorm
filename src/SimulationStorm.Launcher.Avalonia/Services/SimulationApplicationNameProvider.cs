using System.ComponentModel;
using SimulationStorm.Launcher.Presentation.Models;
using SimulationStorm.Launcher.Presentation.Services;

namespace SimulationStorm.Launcher.Avalonia.Services;

public class SimulationApplicationNameProvider : ISimulationApplicationNameProvider
{
    public string GetApplicationName(SimulationType simulationType) => simulationType switch
    {
        SimulationType.GameOfLife => GameOfLife.Avalonia.Startup.AvaloniaConfiguration.ApplicationName,
        _ => throw new InvalidEnumArgumentException(nameof(simulationType), (int)simulationType, typeof(SimulationType))
    };
}