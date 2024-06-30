using System;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.History.Presentation.Models;

public class HistoryRecord<TSave>(SimulationCommand executedCommand, TSave save, TimeSpan savingTime)
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;
    
    public TSave Save { get; } = save;

    public TimeSpan SavingTime { get; } = savingTime;
}