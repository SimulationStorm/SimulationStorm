using System;
using System.Collections.Generic;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public interface IStatsOptions : ICollectionManagerOptions
{
    IEnumerable<ChartType> ChartTypes { get; }

    ChartType DefaultChartType { get; }
    
    IReadOnlyDictionary<ChartType, Type> ViewModelTypesByChartType { get; }
}