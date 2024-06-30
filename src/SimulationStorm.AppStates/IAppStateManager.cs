using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SimulationStorm.AppStates;

public interface IAppStateManager
{
    ReadOnlyObservableCollection<AppState> AppStates { get; }
    
    Task SaveAppStateAsync(string name);

    Task RestoreAppStateAsync(AppState appState);

    Task UpdateAppStateAsync(AppState appState);
    
    Task DeleteAppStateAsync(AppState appState);

    Task DeleteAllAppStatesAsync();
}