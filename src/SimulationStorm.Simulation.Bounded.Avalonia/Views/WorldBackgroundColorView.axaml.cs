using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Bounded.Avalonia.Views;

public partial class WorldBackgroundColorView : UserControl
{
    public WorldBackgroundColorView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<WorldBackgroundColorViewModel>();
    }
}