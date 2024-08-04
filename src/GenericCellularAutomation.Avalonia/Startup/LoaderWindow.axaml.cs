using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Densities.Presentation;
using SimulationStorm.Presentation;
using SimulationStorm.Themes.Presentation;

namespace GenericCellularAutomation.Avalonia.Startup;

public partial class LoaderWindow : WindowExtended
{
    public LoaderWindow() { }
    
    public LoaderWindow(IShutdownService shutdownService)
    {
        InitializeComponent();
        
        Content = new LoaderView(shutdownService);
    }
}