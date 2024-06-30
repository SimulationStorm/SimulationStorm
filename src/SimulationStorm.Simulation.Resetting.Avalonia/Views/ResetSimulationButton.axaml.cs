using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.Resetting.Presentation.Services;

namespace SimulationStorm.Simulation.Resetting.Avalonia.Views;

public partial class ResetSimulationButton : Button
{
    public ResetSimulationButton()
    {
        InitializeComponent();
        
        var simulationManager = DiContainer.Default.GetRequiredService<IResettableSimulationManager>();
        Command = new AsyncRelayCommand(simulationManager.ResetAsync);
    }
}