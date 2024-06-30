using System;

namespace SimulationStorm.Simulation.Statistics.Presentation.Charts;

public interface IChartViewModelFactory
{
    IDisposable CreateChartViewModel(ChartType chartType);
}