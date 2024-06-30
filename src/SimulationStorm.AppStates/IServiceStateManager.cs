using System;

namespace SimulationStorm.AppStates;

public interface IServiceStateManager
{
    Type StateType { get; }
    
    object SaveServiceState();

    void RestoreServiceState(object state);
}