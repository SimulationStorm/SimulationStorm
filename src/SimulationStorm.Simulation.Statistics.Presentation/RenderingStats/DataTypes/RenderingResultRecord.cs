using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;

public class RenderingResultRecord(SimulationCommand? executedCommand, TimeSpan renderingTime)
{
    public SimulationCommand? ExecutedCommand { get; } = executedCommand;

    public TimeSpan RenderingTime { get; } = renderingTime;
}