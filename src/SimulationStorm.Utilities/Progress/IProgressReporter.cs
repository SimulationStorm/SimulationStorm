using System;

namespace SimulationStorm.Utilities.Progress;

public interface IProgressReporter
{
    bool IsProgressReportingEnabled { get; set; }
    
    event EventHandler<CancellableProgressChangedEventArgs>? ProgressChanged;
}