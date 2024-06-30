using System;

namespace SimulationStorm.AppStates;

public interface IEntityFactory
{
    AppState CreateAppState(string name);
    
    AppServiceState CreateAppServiceState(Type stateType, object state);
}