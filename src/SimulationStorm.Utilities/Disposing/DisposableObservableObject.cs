﻿using System;
using System.Reactive.Disposables;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimulationStorm.Utilities.Disposing;

public abstract class DisposableObservableObject : ObservableObject, IDisposable
{
    public bool IsDisposed { get; protected set; }
    
    private readonly CompositeDisposable _disposables = new();

    protected void WithDisposables(Action<CompositeDisposable> action) => action(_disposables);

    public virtual void Dispose()
    {
        if (IsDisposed)
            return;

        _disposables.Dispose();
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}