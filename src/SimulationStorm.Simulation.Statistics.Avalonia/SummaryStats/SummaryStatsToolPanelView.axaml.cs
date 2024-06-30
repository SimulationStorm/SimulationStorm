using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;
using SimulationStorm.ToolPanels.Avalonia;

namespace SimulationStorm.Simulation.Statistics.Avalonia.SummaryStats;

public partial class SummaryStatsToolPanelView : ToolPanelControl
{
    public SummaryStatsToolPanelView()
    {
        InitializeComponent();
        
        var viewModel = DiContainer.Default.GetRequiredService<ISummaryStatsViewModel>();
        DataContext = viewModel;
        
        this.BindChartViewModelToContentControl(viewModel, ChartContentControl);
    }
}