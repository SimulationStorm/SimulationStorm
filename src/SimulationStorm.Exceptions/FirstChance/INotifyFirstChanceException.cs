using System;

namespace SimulationStorm.Exceptions.FirstChance;

public interface INotifyFirstChanceException
{
    event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException;
}