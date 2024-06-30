using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.ViewModels;

namespace SimulationStorm.Simulation.Statistics.Avalonia.CommandExecutionStats;

public partial class CommandExecutionStatsView : Section
{
    public CommandExecutionStatsView()
    {
        InitializeComponent();
        
        var viewModel = DiContainer.Default.GetRequiredService<CommandExecutionStatsViewModel>();
        DataContext = viewModel;
        
        this.BindChartViewModelToContentControl(viewModel, ChartContentControl);
    }
}