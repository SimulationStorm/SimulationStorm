using System;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;

public class SummaryStatsChartViewModelFactory
(
    IServiceProvider serviceProvider,
    ISummaryStatsOptions options
)
    : ChartViewModelFactory(serviceProvider, options);