using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Running.Avalonia.Views;

public partial class AdvanceSimulationButton : Button
{
    public AdvanceSimulationButton()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<SimulationRunnerViewModel>();
    }
}