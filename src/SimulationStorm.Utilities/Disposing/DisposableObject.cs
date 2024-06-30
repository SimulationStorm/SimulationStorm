using System;
using System.Reactive.Disposables;
using Disposable = DotNext.Disposable;

namespace SimulationStorm.Utilities.Disposing;

public abstract class DisposableObject : Disposable
{
    private string? ObjectName => GetType().FullName;

    protected CompositeDisposable Disposables { get; } = new();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        if (disposing)
            Disposables.Dispose();
    }

    #region Throw helpers
    protected void ThrowIfDisposing()
    {
        if (IsDisposing)
            throw new ObjectDisposedException(ObjectName, "The object is being disposed.");
    }
    
    protected void ThrowIfDisposed()
    {
        if (IsDisposing)
            throw new ObjectDisposedException(ObjectName, "The object is disposed.");
    }

    protected void ThrowIfDisposingOrDisposed()
    {
        if (IsDisposingOrDisposed)
            throw new ObjectDisposedException(ObjectName, "The object is being disposed or disposed.");
    }
    #endregion
}