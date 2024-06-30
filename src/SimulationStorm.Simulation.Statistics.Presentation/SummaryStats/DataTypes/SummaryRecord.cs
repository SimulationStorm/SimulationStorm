using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;

public class SummaryRecord<TSummary>(SimulationCommand executedCommand, TSummary summary, TimeSpan summarizingTime)
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;

    public TSummary Summary { get; } = summary;
    
    public TimeSpan SummarizingTime { get; } = summarizingTime;
}