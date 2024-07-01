using System;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation;

public interface ISimulation
{
    bool IsOperationProgressReportingEnabled { get; set; }

    event EventHandler<CancellableProgressChangedEventArgs> OperationProgressChanged;
}