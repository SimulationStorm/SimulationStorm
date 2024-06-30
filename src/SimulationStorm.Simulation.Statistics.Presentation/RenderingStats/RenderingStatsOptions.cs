using System;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;

public class RenderingStatsOptions : StatsOptions
{
    public string CommandNumberAxisNameKey { get; init; } = null!;
    
    public string RenderingTimeAxisNameKey { get; init; } = null!;
    
    public TimeSpan RenderingTimeAxisTimeUnit { get; init; }
}