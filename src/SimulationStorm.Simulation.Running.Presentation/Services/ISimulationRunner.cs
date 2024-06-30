using System;
using System.ComponentModel;
using SimulationStorm.Simulation.Running.Presentation.Models;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public interface ISimulationRunner : INotifyPropertyChanged
{
    #region Properties
    SimulationRunningState SimulationRunningState { get; }
    
    int MaxStepsPerSecond { get; set; }
    #endregion
    
    event EventHandler<SimulationAdvancedEventArgs> SimulationAdvanced;

    #region Methods
    void StartSimulation();

    void PauseSimulation();
    #endregion
}