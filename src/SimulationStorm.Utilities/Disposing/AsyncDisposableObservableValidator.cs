using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Disposing;

public abstract class AsyncDisposableObservableValidator : ObservableValidator, IAsyncDisposable
{
    public bool IsDisposed { get; protected set; }
    
    private readonly CompositeDisposable _disposables = new();

    protected void WithDisposables(Action<CompositeDisposable> action) => action(_disposables);

    public virtual ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return ValueTask.CompletedTask;
        
        _disposables.Dispose();
        IsDisposed = true;
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}