using System;
using System.Threading.Tasks;

namespace SimulationStorm.AppStates;

public interface IAsyncServiceStateManager
{
    Type StateType { get; }
    
    Task<object> SaveServiceStateAsync();

    Task RestoreServiceStateAsync(object state);
}