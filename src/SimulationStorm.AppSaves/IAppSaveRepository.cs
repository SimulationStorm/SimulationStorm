using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationStorm.AppSaves.Entities;

namespace SimulationStorm.AppSaves;

public interface IAppSaveRepository
{
    Task AddAppSaveAsync(AppSave appSave);
    
    IEnumerable<AppSave> GetAllAppSaves();

    Task UpdateAppSaveAsync(AppSave appSave);

    Task DeleteAppSaveAsync(AppSave appSave);

    Task DeleteAllAppSavesAsync();
}