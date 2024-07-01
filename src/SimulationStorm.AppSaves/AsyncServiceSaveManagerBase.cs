using System;
using System.Threading.Tasks;

namespace SimulationStorm.AppSaves;

public abstract class AsyncServiceSaveManagerBase<TSave> : IAsyncServiceSaveManager
{
    public Type SaveType { get; } = typeof(TSave);

    #region Public methods
    public async Task<object> SaveServiceAsync()
    {
        var save = await SaveServiceAsyncCore();
        return (TSave)save!;
    }

    public Task RestoreServiceSaveAsync(object save) => RestoreServiceSaveAsyncCore((TSave)save);
    #endregion

    #region Protected methods
    protected abstract Task<TSave> SaveServiceAsyncCore();

    protected abstract Task RestoreServiceSaveAsyncCore(TSave save);
    #endregion
}