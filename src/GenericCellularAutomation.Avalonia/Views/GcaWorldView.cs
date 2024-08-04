using GenericCellularAutomation.Presentation;
using GenericCellularAutomation.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.Cellular.Avalonia.Views;

namespace GenericCellularAutomation.Avalonia.Views;

public sealed class GcaWorldView : BoundedCellularSimulationWorldView
{
    public GcaWorldView()
    {
        var viewModel = DiContainer.Default.GetRequiredService<GcaWorldViewModel>();
        Initialize(viewModel);
    }
}