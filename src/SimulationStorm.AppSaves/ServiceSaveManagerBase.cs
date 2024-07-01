using System;

namespace SimulationStorm.AppSaves;

public abstract class ServiceSaveManagerBase<TSave> : IServiceSaveManager
{
    public Type SaveType { get; } = typeof(TSave);

    #region Public methods
    public object SaveService() => SaveServiceCore()!;

    public void RestoreServiceSave(object save) => RestoreServiceSaveCore((TSave)save);
    #endregion

    #region Protected methods
    protected abstract TSave SaveServiceCore();

    protected abstract void RestoreServiceSaveCore(TSave save);
    #endregion
}