using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotNext.Collections.Generic;
using DynamicData;
using SimulationStorm.AppSaves.Operations;

namespace SimulationStorm.AppSaves;

public class AppSaveManager : IAppSaveManager
{
    public ReadOnlyObservableCollection<AppSave> AppSaves { get; }

    #region Fields
    private readonly IAppSavesEntityFactory _appSavesEntityFactory;
    
    private readonly IAppSaveRepository _appSaveRepository;
    
    private readonly ObservableCollection<AppSave> _appSaves = [];

    private readonly IEnumerable<IServiceSaveManager> _serviceSaveManagers;

    private readonly IEnumerable<IAsyncServiceSaveManager> _asyncServiceSaveManagers;

    private readonly IEnumerable<IAppSaveRestoringOperation> _appSaveRestoringOperations;
    #endregion

    public AppSaveManager
    (
        IAppSavesEntityFactory appSavesEntityFactory,
        IAppSaveRepository appSaveRepository,
        IEnumerable<IServiceSaveManager> serviceSaveManagers,
        IEnumerable<IAsyncServiceSaveManager> asyncServiceSaveManagers,
        IEnumerable<IAppSaveRestoringOperation> appSaveRestoringOperations)
    {
        _appSavesEntityFactory = appSavesEntityFactory;
        _appSaveRepository = appSaveRepository;
        _serviceSaveManagers = serviceSaveManagers;
        _asyncServiceSaveManagers = asyncServiceSaveManagers;
        _appSaveRestoringOperations = appSaveRestoringOperations;

        AppSaves = new ReadOnlyObservableCollection<AppSave>(_appSaves);
        
        LoadAllAppSavesFromRepository();
    }

    #region Public methods
    public Task SaveAppAsync(string saveName) => Task.Run(async () =>
    {
        var appSave = _appSavesEntityFactory.CreateAppSave(saveName);

        _serviceSaveManagers.ForEach(serviceSaveManager =>
        {
            var saveType = serviceSaveManager.SaveType;
            var save = serviceSaveManager.SaveService();

            var appServiceSave = _appSavesEntityFactory.CreateServiceSave(saveType, save);
            appSave.ServiceSaves.Add(appServiceSave);
        });

        foreach (var asyncServiceSaveManager in _asyncServiceSaveManagers)
        {
            var saveType = asyncServiceSaveManager.SaveType;
            var save = await asyncServiceSaveManager
                .SaveServiceAsync()
                .ConfigureAwait(false);
            
            var appServiceSave = _appSavesEntityFactory.CreateServiceSave(saveType, save);
            appSave.ServiceSaves.Add(appServiceSave);
        }

        await _appSaveRepository
            .AddAppSaveAsync(appSave)
            .ConfigureAwait(false);
        
        _appSaves.Add(appSave);
    });

    /// <remarks>
    /// Due to using LazyLoadProxies(), accessing serviceSave.Save may take significant time;
    /// because of it, all following operations are executed in task.
    /// </remarks>
    public Task RestoreAppSaveAsync(AppSave appSave) => Task.Run(async () =>
    {
        _appSaveRestoringOperations.ForEach(operation => operation.OnAppSaveRestoring());
        
        _serviceSaveManagers.ForEach(serviceSaveManager =>
        {
            var serviceSave = appSave.ServiceSaves.First(x => x.SaveObjectType == serviceSaveManager.SaveType);
            serviceSaveManager.RestoreServiceSave(serviceSave.SaveObject);
        });
    
        foreach (var asyncServiceSaveManager in _asyncServiceSaveManagers)
        {
            var serviceSave = appSave.ServiceSaves.First(x => x.SaveObjectType == asyncServiceSaveManager.SaveType);
            
            await asyncServiceSaveManager
                .RestoreServiceSaveAsync(serviceSave.SaveObject)
                .ConfigureAwait(false);
        }
    });

    public Task UpdateAppSaveAsync(AppSave appSave) => _appSaveRepository.UpdateAppSaveAsync(appSave);

    public async Task DeleteAppSaveAsync(AppSave appSave)
    {
        await _appSaveRepository
            .DeleteAppSaveAsync(appSave)
            .ConfigureAwait(false);
        
        _appSaves.Remove(appSave);
    }

    public async Task DeleteAllAppSavesAsync()
    {
        await _appSaveRepository
            .DeleteAllAppSavesAsync()
            .ConfigureAwait(false);
        
        _appSaves.Clear();
    }
    #endregion

    private void LoadAllAppSavesFromRepository() => _appSaves.AddRange(_appSaveRepository.GetAllAppSaves());
}