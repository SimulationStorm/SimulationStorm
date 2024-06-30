using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;

namespace SimulationStorm.GameOfLife.Presentation.Stats;

public class StatsRecordModel(SimulationCommand command, TimeSpan summarizingTime, int deadCellCount, int aliveCellCount) :
    SummaryRecordModelBase(command, summarizingTime)
{
    public int DeadCellCount { get; } = deadCellCount;

    public int AliveCellCount { get; } = aliveCellCount;
}