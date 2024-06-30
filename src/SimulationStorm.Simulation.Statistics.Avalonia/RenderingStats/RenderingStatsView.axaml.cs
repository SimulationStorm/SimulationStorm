using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.ViewModels;

namespace SimulationStorm.Simulation.Statistics.Avalonia.RenderingStats;

public partial class RenderingStatsView : Section
{
    public RenderingStatsView()
    {
        InitializeComponent();

        var viewModel = DiContainer.Default.GetRequiredService<RenderingStatsViewModel>();
        DataContext = viewModel;
        
        this.BindChartViewModelToContentControl(viewModel, ChartContentControl);
    }
}