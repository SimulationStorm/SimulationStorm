using SimulationStorm.Launcher.Presentation.Models;

namespace SimulationStorm.Launcher.Presentation.Services;

public interface ISimulationApplicationNameProvider
{
    string GetApplicationName(SimulationType simulationType);
}