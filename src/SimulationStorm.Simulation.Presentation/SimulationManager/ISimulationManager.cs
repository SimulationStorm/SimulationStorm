using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public interface ISimulationManager
{
    bool IsCommandProgressReportingEnabled { get; set; }
    
    #region Events
    /// <summary>
    /// Occurs after the command has been scheduled to execution.
    /// </summary>
    event EventHandler<SimulationCommandEventArgs>? CommandScheduling;
    
    /// <summary>
    /// Occurs before the start of the command execution.
    /// </summary>
    event EventHandler<SimulationCommandEventArgs>? CommandStarting;
 
    /// <summary>
    /// Occurs when the command progress changes.
    /// </summary>
    event EventHandler<SimulationCommandProgressChangedEventArgs>? CommandProgressChanged;

    /// <summary>
    /// Occurs after the command has been executed.
    /// </summary>
    event EventHandler<SimulationCommandCompletedEventArgs>? CommandCompleted;
    #endregion
    
    #region Methods
    Task ScheduleCommandAsync(SimulationCommand command);

    Task ClearScheduledCommandsAsync();
    #endregion
}