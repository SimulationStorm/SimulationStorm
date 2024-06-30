﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public interface ISimulationManager
{
    ReadOnlyObservableCollection<SimulationCommand> CommandQueue { get; }
    
    /// <summary>
    /// Occurs before the start of the command execution.
    /// </summary>
    event EventHandler<SimulationCommandEventArgs>? CommandStarting;

    /// <summary>
    /// Occurs after the command has been executed.
    /// </summary>
    event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;

    // Task ClearCommandQueueAsync();

    // Task WaitForAllQueuedCommandsExecutingAsync(); // to implement this, we can use Task.WhenAll(queuedCommands.Tasks)
}