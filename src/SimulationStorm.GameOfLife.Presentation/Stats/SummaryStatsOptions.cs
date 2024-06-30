using SimulationStorm.Simulation.Statistics.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public class SummaryStatsOptions : StatsOptions, ISummaryStatsOptions
{
    public string CommandNumberAxisNameKey { get; init; } = null!;
    
    public string AliveCellCountAxisNameKey { get; init; } = null!;

    public string DeadCellsStringKey { get; init; } = null!;

    public string AliveCellsStringKey { get; init; } = null!;
}