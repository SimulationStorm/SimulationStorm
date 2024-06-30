using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;

public class CommandExecutionResultRecordModel(SimulationCommand executedCommand, TimeSpan commandExecutionTime) : IndexedObject
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;

    public TimeSpan CommandExecutionTime { get; } = commandExecutionTime;
}