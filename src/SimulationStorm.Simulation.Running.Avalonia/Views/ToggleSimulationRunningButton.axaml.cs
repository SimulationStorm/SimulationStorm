using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Running.Avalonia.Views;

public partial class ToggleRunningButton : Button
{
    public ToggleRunningButton()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<SimulationRunnerViewModel>();
    }
}