using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Disposing;

public abstract class AsyncDisposableObservableObject : ObservableObject, IAsyncDisposable
{
    public bool IsDisposed { get; protected set; }

    protected CompositeDisposable Disposables { get; } = new();

    public virtual ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return ValueTask.CompletedTask;
        
        Disposables.Dispose();
        IsDisposed = true;
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}