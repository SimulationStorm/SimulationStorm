using System;
using System.Threading.Tasks;

namespace SimulationStorm.Utilities.Disposing;

public abstract class AsyncDisposableObject : DisposableObject, IAsyncDisposable
{
    public new ValueTask DisposeAsync() => base.DisposeAsync();
}