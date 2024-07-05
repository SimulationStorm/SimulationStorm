using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotNext.Collections.Generic;
using DynamicData;
using SimulationStorm.AppSaves.Entities;
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
    
    private readonly IEnumerable<IAppSaveRestoredOperation> _appSaveRestoredOperations;
    #endregion

    public AppSaveManager
    (
        IAppSavesEntityFactory appSavesEntityFactory,
        IAppSaveRepository appSaveRepository,
        IEnumerable<IServiceSaveManager> serviceSaveManagers,
        IEnumerable<IAsyncServiceSaveManager> asyncServiceSaveManagers,
        IEnumerable<IAppSaveRestoringOperation> appSaveRestoringOperations,
        IEnumerable<IAppSaveRestoredOperation> appSaveRestoredOperations)
    {
        _appSavesEntityFactory = appSavesEntityFactory;
        _appSaveRepository = appSaveRepository;
        _serviceSaveManagers = serviceSaveManagers;
        _asyncServiceSaveManagers = asyncServiceSaveManagers;
        _appSaveRestoringOperations = appSaveRestoringOperations;
        _appSaveRestoredOperations = appSaveRestoredOperations;

        AppSaves = new ReadOnlyObservableCollection<AppSave>(_appSaves);
        
        LoadAllAppSavesFromRepository();
    }

    #region Public methods
    public Task SaveAppAsync(string saveName) => Task.Run(async () =>
    {
        var appSave = _appSavesEntityFactory.CreateAppSave(saveName);

        foreach (var serviceSave in CreateServiceSaves())
            appSave.ServiceSaves.Add(serviceSave);

        await foreach (var serviceSave in CreateAsyncServiceSavesAsync()
                           .ConfigureAwait(false))
            appSave.ServiceSaves.Add(serviceSave);

        await _appSaveRepository
            .AddAppSaveAsync(appSave)
            .ConfigureAwait(false);
        
        _appSaves.Add(appSave);
    });
    
    public Task RestoreAppSaveAsync(AppSave appSave) => Task.Run(async () =>
    {
        ExecuteAppSaveRestoringOperations();

        var serviceSaveObjectsByType = appSave.ServiceSaves
            .ToDictionary(k => k.SaveObjectType, v => v.SaveObject);
        
        RestoreServiceSaves(serviceSaveObjectsByType);
        
        await RestoreAsyncServiceSavesAsync(serviceSaveObjectsByType)
            .ConfigureAwait(false);
        
        ExecuteAppSaveRestoredOperations();
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

    #region Private methods
    private void LoadAllAppSavesFromRepository() => _appSaves.AddRange(_appSaveRepository.GetAllAppSaves());
    
    #region Operations
    private void ExecuteAppSaveRestoringOperations() =>
        _appSaveRestoringOperations.ForEach(operation => operation.OnAppSaveRestoring());
    
    private void ExecuteAppSaveRestoredOperations() =>
        _appSaveRestoredOperations.ForEach(operation => operation.OnAppSaveRestored());
    #endregion

    #region Services saving
    private IEnumerable<ServiceSave> CreateServiceSaves()
    {
        foreach (var serviceSaveManager in _serviceSaveManagers)
        {
            var saveType = serviceSaveManager.SaveType;
            var save = serviceSaveManager.SaveService();

            yield return _appSavesEntityFactory.CreateServiceSave(saveType, save);
        }
    }

    private async IAsyncEnumerable<ServiceSave> CreateAsyncServiceSavesAsync()
    {
        foreach (var asyncServiceSaveManager in _asyncServiceSaveManagers)
        {
            var saveType = asyncServiceSaveManager.SaveType;
            
            var save = await asyncServiceSaveManager
                .SaveServiceAsync()
                .ConfigureAwait(false);
            
            yield return _appSavesEntityFactory.CreateServiceSave(saveType, save);
        }
    }
    #endregion

    #region Services restoring
    private void RestoreServiceSaves(IDictionary<Type, object> serviceSaveObjectsByType)
    {
        foreach (var serviceSaveManager in _serviceSaveManagers)
        {
            var save = serviceSaveObjectsByType[serviceSaveManager.SaveType];
            serviceSaveManager.RestoreServiceSave(save);
        }
    }
    
    private async Task RestoreAsyncServiceSavesAsync(IDictionary<Type, object> serviceSaveObjectsByType)
    {
        foreach (var asyncServiceSaveManager in _asyncServiceSaveManagers)
        {
            var save = serviceSaveObjectsByType[asyncServiceSaveManager.SaveType];
            
            await asyncServiceSaveManager
                .RestoreServiceSaveAsync(save)
                .ConfigureAwait(false);
        }
    }
    #endregion
    #endregion
}