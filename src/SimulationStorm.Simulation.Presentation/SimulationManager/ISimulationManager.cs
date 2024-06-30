using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public interface ISimulationManager
{
    ReadOnlyObservableCollection<SimulationCommand> CommandQueue { get; }
    
    /// <summary>
    /// Occurs before the start of the command execution.
    /// </summary>
    event EventHandler<SimulationCommandExecutingEventArgs>? CommandExecuting;

    /// <summary>
    /// Occurs after the command has been executed.
    /// </summary>
    event EventHandler<SimulationCommandExecutedEventArgs>? CommandExecuted;

    // Task ClearCommandQueueAsync();

    // Task WaitForAllQueuedCommandsExecutingAsync(); // to implement this, we can use Task.WhenAll(queuedCommands.Tasks)
}