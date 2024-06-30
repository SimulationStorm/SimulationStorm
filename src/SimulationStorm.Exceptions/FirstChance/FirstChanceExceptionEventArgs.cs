using System;

namespace SimulationStorm.Exceptions.FirstChance;

public class FirstChanceExceptionEventArgs(Exception exception) : EventArgs
{
    public Exception Exception { get; } = exception;
}