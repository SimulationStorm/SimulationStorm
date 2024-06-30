using Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.GameOfLife.Presentation.ViewModels;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class CellColorsView : UserControl
{
    public CellColorsView()
    {
        InitializeComponent();
        
        this.ResolveViewModelFromDefaultDiContainer<CellColorsViewModel>();
    }
}