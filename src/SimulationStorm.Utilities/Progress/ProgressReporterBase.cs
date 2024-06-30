using System;

namespace SimulationStorm.Utilities.Progress;

public abstract class ProgressReporterBase : IProgressReporter
{
    public bool IsProgressReportingEnabled { get; set; }
    
    public event EventHandler<CancellableProgressChangedEventArgs>? ProgressChanged;
    
    protected int Progress { get; set; }
    
    #region Protected methods
    protected void ResetProgress() => Progress = 0;
    
    protected void ReportProgress() =>
        ProgressChanged?.Invoke(this, new CancellableProgressChangedEventArgs(Progress));
    
    protected bool ReportProgressAndGetCancel()
    {
        var eventArgs = new CancellableProgressChangedEventArgs(Progress);
        ProgressChanged?.Invoke(this, eventArgs);
        return eventArgs.Cancel;
    }
    #endregion
}