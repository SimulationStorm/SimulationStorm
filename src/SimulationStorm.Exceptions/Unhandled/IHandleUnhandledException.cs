namespace SimulationStorm.Exceptions.Unhandled;

public interface IHandleUnhandledException
{
    void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e);
}