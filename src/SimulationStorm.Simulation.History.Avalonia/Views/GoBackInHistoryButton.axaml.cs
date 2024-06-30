using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.History.Presentation.ViewModels;

namespace SimulationStorm.Simulation.History.Avalonia.Views;

public partial class GoBackInHistoryButton : Button
{
    public GoBackInHistoryButton()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<IHistoryViewModel>();
    }
}