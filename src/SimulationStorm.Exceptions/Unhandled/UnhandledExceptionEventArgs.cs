using System;

namespace SimulationStorm.Exceptions.Unhandled;

public class UnhandledExceptionEventArgs(Exception exception, bool canBeHandled) : EventArgs
{
    #region Properties
    public Exception Exception { get; } = exception;

    public bool CanBeHandled { get; } = canBeHandled;

    public bool Handled
    {
        get => _handled;
        set
        {
            if (value && !CanBeHandled)
                throw new InvalidOperationException("The exception can not be handled.");

            _handled = value;
        }
    }
    #endregion
    
    private bool _handled;
}