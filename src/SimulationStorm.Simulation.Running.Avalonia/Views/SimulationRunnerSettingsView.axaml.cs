using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Running.Avalonia.Views;

public partial class SimulationRunnerSettingsView : Section
{
    public SimulationRunnerSettingsView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<SimulationRunnerViewModel>();
    }
}