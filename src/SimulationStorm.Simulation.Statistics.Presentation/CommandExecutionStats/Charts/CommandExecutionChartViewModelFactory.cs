using System;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;

public class CommandExecutionChartViewModelFactory
(
    IServiceProvider serviceProvider,
    CommandExecutionStatsOptions options
)
    : ChartViewModelFactory(serviceProvider, options);