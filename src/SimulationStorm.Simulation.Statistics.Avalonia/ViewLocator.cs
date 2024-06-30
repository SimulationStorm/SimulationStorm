using SimulationStorm.Avalonia;
using SimulationStorm.Simulation.Statistics.Avalonia.RenderingStats;
using SimulationStorm.Simulation.Statistics.Avalonia.SummaryStats;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;
using CommandExecutionStatsBarChartView = SimulationStorm.Simulation.Statistics.Avalonia.CommandExecutionStats.CommandExecutionStatsBarChartView;
using CommandExecutionStatsLineChartView = SimulationStorm.Simulation.Statistics.Avalonia.CommandExecutionStats.CommandExecutionStatsLineChartView;
using CommandExecutionStatsView = SimulationStorm.Simulation.Statistics.Avalonia.CommandExecutionStats.CommandExecutionStatsView;

namespace SimulationStorm.Simulation.Statistics.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<ISummaryStatsViewModel, SummaryStatsToolPanelView>();
        
        Register<CommandExecutionStatsViewModel, CommandExecutionStatsView>();
        Register<CommandExecutionLineChartViewModel, CommandExecutionStatsLineChartView>();
        Register<CommandExecutionBarChartViewModel, CommandExecutionStatsBarChartView>();
        // Register<CommandExecutionStatisticsPieChartViewModel, CommandExecutionStatisticsPieChartChartView>();
        
        Register<RenderingStatsViewModel, RenderingStatsView>();
        Register<RenderingLineChartViewModel, RenderingStatsLineChartView>();
        Register<RenderingBarChartViewModel, RenderingStatsBarChartView>();
        // Register<RenderingPieChartViewModel, RenderingStatisticsPieChartChartView>();
    }
}