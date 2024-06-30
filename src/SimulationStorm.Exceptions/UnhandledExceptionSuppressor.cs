using SimulationStorm.Exceptions.Unhandled;

namespace SimulationStorm.Exceptions;

public class UnhandledExceptionSuppressor : IHandleUnhandledException
{
    public void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.CanBeHandled)
            e.Handled = true;
    }
}