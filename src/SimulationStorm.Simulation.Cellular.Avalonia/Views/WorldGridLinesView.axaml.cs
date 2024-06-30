using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Cellular.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Cellular.Avalonia.Views;

public partial class WorldGridLinesView : UserControl
{
    public WorldGridLinesView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<WorldGridLinesViewModel>();
    }
}