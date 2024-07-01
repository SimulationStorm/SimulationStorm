using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Disposing;

/// <summary>
/// Provides observability and the implementation of dispose pattern.
/// </summary>
/// <remarks>Based on <see cref="Disposable"/></remarks>
public abstract class DisposableObservableObject : ObservableObject, IDisposable
{
    #region Constants
    private const int NotDisposedState = 0;
    
    private const int DisposingState = 1;
    
    private const int DisposedState = 2;
    #endregion

    private volatile int state;

    #region Properties
    /// <summary>
    /// Indicates that this object is disposed.
    /// </summary>
    protected bool IsDisposed => state is DisposedState;

    /// <summary>
    /// Indicates that <see cref="DisposeAsync()"/> is called but not yet completed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool IsDisposing => state is DisposingState;

    /// <summary>
    /// Indicates that <see cref="DisposeAsync()"/> is called.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool IsDisposingOrDisposed => state is not NotDisposedState;
    
    protected CompositeDisposable Disposables { get; } = new();
    #endregion

    #region Methods
    /// <summary>
    /// Releases all resources associated with this object.
    /// </summary>
    [SuppressMessage("Design", "CA1063", Justification = "No need to call Dispose(true) multiple times")]
    public void Dispose()
    {
        Dispose(TryBeginDispose());
        GC.SuppressFinalize(this);
    }
    
    #region Protected methods
    /// <summary>
    /// Releases managed and unmanaged resources associated with this object.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> if called from <see cref="Dispose()"/>; <see langword="false"/> if called from finalizer <see cref="Finalize()"/>.</param>
    protected virtual void Dispose(bool disposing) => state = DisposedState;

    /// <summary>
    /// Releases managed resources associated with this object asynchronously.
    /// </summary>
    /// <remarks>
    /// This method makes sense only if derived class implements <see cref="IAsyncDisposable"/> interface.
    /// </remarks>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    protected virtual ValueTask DisposeAsyncCore()
    {
        Dispose(true);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Releases managed resources associated with this object asynchronously.
    /// </summary>
    /// <remarks>
    /// If derived class implements <see cref="IAsyncDisposable"/> then <see cref="IAsyncDisposable.DisposeAsync"/>
    /// can be trivially implemented through delegation of the call to this method.
    /// </remarks>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    protected ValueTask DisposeAsync() => Interlocked.CompareExchange(ref state, DisposingState, NotDisposedState) switch
    {
        NotDisposedState => DisposeAsyncImpl(),
        DisposingState => DisposeAsyncCore(),
        _ => ValueTask.CompletedTask,
    };

    /// <summary>
    /// Starts disposing this object.
    /// </summary>
    /// <returns><see langword="true"/> if cleanup operations can be performed; <see langword="false"/> if the object is already disposing.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool TryBeginDispose() =>
        Interlocked.CompareExchange(ref state, DisposingState, NotDisposedState) is NotDisposedState;

    #endregion

    private async ValueTask DisposeAsyncImpl()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }
    #endregion

    /// <summary>
    /// Finalizes this object.
    /// </summary>
    ~DisposableObservableObject() => Dispose(false);
}