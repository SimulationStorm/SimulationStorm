using System;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;

public class RenderingChartViewModelFactory
(
    IServiceProvider serviceProvider,
    RenderingStatsOptions options
)
    : ChartViewModelFactory(serviceProvider, options);