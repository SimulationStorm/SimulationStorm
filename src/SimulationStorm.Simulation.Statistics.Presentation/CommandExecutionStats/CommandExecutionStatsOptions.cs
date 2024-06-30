using System;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats;

public class CommandExecutionStatsOptions : StatsOptions
{
    public string CommandNumberAxisNameKey { get; init; } = null!;
    
    public string ExecutionTimeAxisNameKey { get; init; } = null!;
    
    public TimeSpan ExecutionTimeAxisTimeUnit { get; init; }
}