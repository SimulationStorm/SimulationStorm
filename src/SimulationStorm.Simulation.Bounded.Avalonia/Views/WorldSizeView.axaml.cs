using Avalonia;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Bounded.Avalonia.Views;

public partial class WorldSizeView : Section
{
    public static readonly StyledProperty<string?> MeasureUnitTextProperty =
        AvaloniaProperty.Register<WorldSizeView, string?>(nameof(MeasureUnitText));

    public string? MeasureUnitText
    {
        get => GetValue(MeasureUnitTextProperty);
        set => SetValue(MeasureUnitTextProperty, value);
    }
    
    public WorldSizeView()
    {
        InitializeComponent();

        this.ResolveViewModelFromDefaultDiContainer<IWorldSizeViewModel>();
    }
}