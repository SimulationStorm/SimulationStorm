using SimulationStorm.Avalonia;
using SimulationStorm.Launcher.Avalonia.Views;
using SimulationStorm.Launcher.Presentation.ViewModels;

namespace SimulationStorm.Launcher.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<MainViewModel, MainView>();
    }
}