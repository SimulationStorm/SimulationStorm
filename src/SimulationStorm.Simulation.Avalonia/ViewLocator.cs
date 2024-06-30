using SimulationStorm.Avalonia;
using SimulationStorm.Simulation.Avalonia.Views;
using SimulationStorm.Simulation.Presentation.Camera;

namespace SimulationStorm.Simulation.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<CameraSettingsViewModel, CameraToolPanelView>();
    }
}