using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Avalonia.Views;

public partial class CommandQueueView : Section
{
    public CommandQueueView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<CommandQueueViewModel>();
    }
}