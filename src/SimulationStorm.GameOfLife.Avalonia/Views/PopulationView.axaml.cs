using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.GameOfLife.Presentation.Population;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class PopulationView : Section
{
    public PopulationView()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<PopulationViewModel>();
    }
}