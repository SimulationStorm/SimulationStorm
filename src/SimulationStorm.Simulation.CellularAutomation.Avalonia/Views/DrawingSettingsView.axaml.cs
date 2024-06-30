using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

namespace SimulationStorm.Simulation.CellularAutomation.Avalonia.Views;

public partial class DrawingSettingsView : Section
{
    public static readonly StyledProperty<IDataTemplate?> CellStateTemplateProperty =
        AvaloniaProperty.Register<DrawingSettingsView, IDataTemplate?>(nameof(CellStateTemplate));

    public IDataTemplate? CellStateTemplate
    {
        get => GetValue(CellStateTemplateProperty);
        set => SetValue(CellStateTemplateProperty, value);
    }

    private readonly IDrawingSettingsViewModel _viewModel;
    
    public DrawingSettingsView()
    {
        InitializeComponent();

        DataContext = _viewModel = DiContainer.Default.GetRequiredService<IDrawingSettingsViewModel>();
    }

    // [WORKAROUND] For some reason cell state buttons do not receive command if it assigned in XAML ...
    private void OnCellStateButtonLoaded(object? sender, RoutedEventArgs _)
    {
        var cellStateButton = (Button)sender!;
        cellStateButton.Command = _viewModel.ChangeBrushCellStateCommand;
    }
}