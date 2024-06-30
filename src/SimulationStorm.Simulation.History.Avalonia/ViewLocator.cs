using SimulationStorm.Avalonia;
using SimulationStorm.Simulation.History.Avalonia.Views;
using SimulationStorm.Simulation.History.Presentation.ViewModels;

namespace SimulationStorm.Simulation.History.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<IHistoryViewModel, HistoryToolPanelView>();
    }
}