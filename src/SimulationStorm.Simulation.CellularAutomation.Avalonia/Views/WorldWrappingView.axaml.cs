using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

namespace SimulationStorm.Simulation.CellularAutomation.Avalonia.Views;

public partial class WorldWrappingView : Section
{
    public WorldWrappingView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<IWorldWrappingViewModel>();
    }
}