using System;
using System.Threading.Tasks;

namespace SimulationStorm.Utilities.Disposing;

public abstract class AsyncDisposableObservableObject : DisposableObservableObject, IAsyncDisposable
{
    public new ValueTask DisposeAsync() => base.DisposeAsync();
}