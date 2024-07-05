using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SimulationStorm.AppSaves.Entities;

namespace SimulationStorm.AppSaves;

public interface IAppSaveManager
{
    ReadOnlyObservableCollection<AppSave> AppSaves { get; }

    #region Methods
    Task SaveAppAsync(string saveName);

    Task RestoreAppSaveAsync(AppSave appSave);

    Task UpdateAppSaveAsync(AppSave appSave);
    
    Task DeleteAppSaveAsync(AppSave appSave);

    Task DeleteAllAppSavesAsync();
    #endregion
}