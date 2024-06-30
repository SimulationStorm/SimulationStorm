using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimulationStorm.AppStates;

public interface IAppStateRepository
{
    Task AddAsync(AppState appState);
    
    IEnumerable<AppState> GetAll();

    Task UpdateAsync(AppState appState);

    Task DeleteAsync(AppState appState);

    Task DeleteAllAsync();
}