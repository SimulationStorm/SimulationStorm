using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;

public abstract class SummaryRecordModelBase(SimulationCommand executedCommand, TimeSpan summarizingTime) : IndexedObject
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;

    public TimeSpan SummarizingTime { get; } = summarizingTime;
}