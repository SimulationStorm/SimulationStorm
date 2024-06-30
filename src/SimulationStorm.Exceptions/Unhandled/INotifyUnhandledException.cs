using System;

namespace SimulationStorm.Exceptions.Unhandled;

public interface INotifyUnhandledException
{
    event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;
}