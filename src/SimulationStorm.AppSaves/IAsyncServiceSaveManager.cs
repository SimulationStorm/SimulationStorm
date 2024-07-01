using System;
using System.Threading.Tasks;

namespace SimulationStorm.AppSaves;

public interface IAsyncServiceSaveManager
{
    Type SaveType { get; }
    
    Task<object> SaveServiceAsync();

    Task RestoreServiceSaveAsync(object save);
}