using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;

public class CommandExecutionResultRecord(SimulationCommand executedCommand, TimeSpan commandExecutionTime)
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;

    public TimeSpan CommandExecutionTime { get; } = commandExecutionTime;
}