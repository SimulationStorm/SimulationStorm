using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace SimulationStorm.Simulation.Statistics.Presentation.Charts;

public class ChartViewModelFactory(IServiceProvider serviceProvider, IStatsOptions options) : IChartViewModelFactory
{
    public IDisposable CreateChartViewModel(ChartType chartType)
    {
        if (options.ViewModelTypesByChartType.TryGetValue(chartType, out var viewModelType))
            return (IDisposable)serviceProvider.GetRequiredService(viewModelType);
        
        throw new NotSupportedException($"There is no registered view model for the chart of type {chartType}.");
    }
}