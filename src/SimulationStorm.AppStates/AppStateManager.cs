using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotNext.Collections.Generic;
using DynamicData;
using SimulationStorm.AppStates.Operations;

namespace SimulationStorm.AppStates;

public class AppStateManager : IAppStateManager
{
    public ReadOnlyObservableCollection<AppState> AppStates { get; }

    #region Fields
    private readonly IEntityFactory _entityFactory;
    
    private readonly IAppStateRepository _appStateRepository;
    
    private readonly ObservableCollection<AppState> _appStates = [];

    private readonly IEnumerable<IServiceStateManager> _serviceStateManagers;

    private readonly IEnumerable<IAsyncServiceStateManager> _asyncServiceStateManagers;

    private readonly IEnumerable<IAppStateRestoringOperation> _appStateRestoringOperations;
    #endregion

    public AppStateManager
    (
        IEntityFactory entityFactory,
        IAppStateRepository appStateRepository,
        IEnumerable<IServiceStateManager> serviceStateManagers,
        IEnumerable<IAsyncServiceStateManager> asyncServiceStateManagers,
        IEnumerable<IAppStateRestoringOperation> appStateRestoringOperations)
    {
        _entityFactory = entityFactory;
        _appStateRepository = appStateRepository;
        _serviceStateManagers = serviceStateManagers;
        _asyncServiceStateManagers = asyncServiceStateManagers;
        _appStateRestoringOperations = appStateRestoringOperations;

        AppStates = new ReadOnlyObservableCollection<AppState>(_appStates);
        
        LoadAppStates();
    }

    #region Public methods
    public Task SaveAppStateAsync(string name) => Task.Run(async () =>
    {
        var appState = _entityFactory.CreateAppState(name);

        _serviceStateManagers.ForEach(serviceStateManager =>
        {
            var stateType = serviceStateManager.StateType;
            var state = serviceStateManager.SaveServiceState();

            var appServiceState = _entityFactory.CreateAppServiceState(stateType, state);
            appState.ServiceStates.Add(appServiceState);
        });

        foreach (var asyncServiceStateManager in _asyncServiceStateManagers)
        {
            var stateType = asyncServiceStateManager.StateType;
            var state = await asyncServiceStateManager
                .SaveServiceStateAsync()
                .ConfigureAwait(false);
            
            var appServiceState = _entityFactory.CreateAppServiceState(stateType, state);
            appState.ServiceStates.Add(appServiceState);
        }

        await _appStateRepository
            .AddAsync(appState)
            .ConfigureAwait(false);
        
        _appStates.Add(appState);
    });

    // [NOTE] Due to using LazyLoadProxies(), accessing serviceState.State may take significant time;
    // because of it, all following operations are executed in task.
    public Task RestoreAppStateAsync(AppState appState) => Task.Run(async () =>
    {
        _appStateRestoringOperations.ForEach(operation => operation.OnAppStateRestoring());
        
        _serviceStateManagers.ForEach(serviceStateManager =>
        {
            var serviceState = appState.ServiceStates.First(x => x.StateType == serviceStateManager.StateType);
            serviceStateManager.RestoreServiceState(serviceState.State);
        });
    
        foreach (var asyncServiceStateManager in _asyncServiceStateManagers)
        {
            var serviceState = appState.ServiceStates.First(x => x.StateType == asyncServiceStateManager.StateType);
            
            await asyncServiceStateManager
                .RestoreServiceStateAsync(serviceState.State)
                .ConfigureAwait(false);
        }
    });

    public Task UpdateAppStateAsync(AppState appState) => _appStateRepository.UpdateAsync(appState);

    public async Task DeleteAppStateAsync(AppState appState)
    {
        await _appStateRepository
            .DeleteAsync(appState)
            .ConfigureAwait(false);
        
        _appStates.Remove(appState);
    }

    public async Task DeleteAllAppStatesAsync()
    {
        await _appStateRepository
            .DeleteAllAsync()
            .ConfigureAwait(false);
        
        _appStates.Clear();
    }
    #endregion

    private void LoadAppStates() => _appStates.AddRange(_appStateRepository.GetAll());
}