using System;
using System.Collections.Generic;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public abstract class StatsOptions : CollectionManagerOptions, IStatsOptions
{
    public IEnumerable<ChartType> ChartTypes { get; init; } = null!;

    public ChartType DefaultChartType { get; init; }

    public IReadOnlyDictionary<ChartType, Type> ViewModelTypesByChartType { get; init; } = null!;
}