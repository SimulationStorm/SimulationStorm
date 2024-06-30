using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;

public class RenderingResultRecordModel(SimulationCommand? executedCommand, TimeSpan renderingTime) : IndexedObject
{
    public SimulationCommand? ExecutedCommand { get; } = executedCommand;

    public TimeSpan RenderingTime { get; } = renderingTime;
}