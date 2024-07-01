using System;
using System.Reactive.Disposables;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Disposing;

public abstract class DisposableObservableObject : ObservableObject, IDisposable
{
    public bool IsDisposed { get; protected set; }

    protected CompositeDisposable Disposables { get; } = new();

    public virtual void Dispose()
    {
        if (IsDisposed)
            return;

        Disposables.Dispose();
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}