using System;
using System.Threading.Tasks;

namespace SimulationStorm.AppStates;

public abstract class AsyncServiceStateManagerBase<TState> : IAsyncServiceStateManager
{
    public Type StateType { get; } = typeof(TState);

    public async Task<object> SaveServiceStateAsync()
    {
        var state = await SaveServiceStateAsyncImpl();
        return (TState)state!;
    }

    public Task RestoreServiceStateAsync(object state) => RestoreServiceStateAsyncImpl((TState)state);

    #region Protected methods
    protected abstract Task<TState> SaveServiceStateAsyncImpl();

    protected abstract Task RestoreServiceStateAsyncImpl(TState state);
    #endregion
}