using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.GameOfLife.Presentation.ViewModels;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class AlgorithmView : Section
{
    public AlgorithmView()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<AlgorithmViewModel>();
    }
}