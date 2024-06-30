using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Presentation.StatusBar;

namespace SimulationStorm.Simulation.Avalonia.Views;

public partial class StatusBarView : UserControl
{
    public StatusBarView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<StatusBarViewModel>();
    }
}