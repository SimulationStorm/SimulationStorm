using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;

namespace SimulationStorm.Simulation.Avalonia.Views;

public partial class SimulationRenderingSettingsView : UserControl
{
    public SimulationRenderingSettingsView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<SimulationRenderingSettingsViewModel>();
    }
}