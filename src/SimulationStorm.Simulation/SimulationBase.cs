using System;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation;

public abstract class SimulationBase
{
    public bool IsOperationProgressReportingEnabled { get; set; }
    
    public event EventHandler<CancellableProgressChangedEventArgs>? OperationProgressChanged;
    
    protected int OperationProgress { get; set; }
    
    #region Protected methods
    protected void ResetOperationProgress() => OperationProgress = 0;
    
    protected void ReportOperationProgress() =>
        OperationProgressChanged?.Invoke(this, new CancellableProgressChangedEventArgs(OperationProgress));
    
    protected bool ReportOperationProgressAndGetCancel()
    {
        var eventArgs = new CancellableProgressChangedEventArgs(OperationProgress);
        OperationProgressChanged?.Invoke(this, eventArgs);
        return eventArgs.Cancel;
    }
    #endregion
}