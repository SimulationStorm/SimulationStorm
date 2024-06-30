using System;

namespace SimulationStorm.AppStates.Persistence;

public class EntityFactory : IEntityFactory
{
    public AppState CreateAppState(string name) => new()
    {
        Name = name
    };
    
    public AppServiceState CreateAppServiceState(Type stateType, object state) => new()
    {
        StateType = stateType,
        State = state
    };
}