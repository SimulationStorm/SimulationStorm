using System;

namespace SimulationStorm.AppStates;

public abstract class ServiceStateManagerBase<TState> : IServiceStateManager
{
    public Type StateType { get; } = typeof(TState);

    #region Public methods
    public object SaveServiceState() => SaveServiceStateImpl()!;

    public void RestoreServiceState(object state) => RestoreServiceStateImpl((TState)state);
    #endregion

    #region Protected methods
    protected abstract TState SaveServiceStateImpl();

    protected abstract void RestoreServiceStateImpl(TState state);
    #endregion
}