using System;
using System.Collections.Generic;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;

namespace SimulationStorm.Simulation.Statistics.Presentation;

public interface IStatsViewModel : ICollectionManagerViewModel
{
    IEnumerable<ChartType> ChartTypes { get; }

    ChartType CurrentChartType { get; set; }
    
    IDisposable? CurrentChartViewModel { get; set; }
}