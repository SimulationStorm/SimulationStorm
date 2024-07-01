using System;

namespace SimulationStorm.Utilities.Disposing;

public static class DisposableObjectExtensions
{
    public static void ThrowIfDisposing(this IDisposable disposable, bool isDisposing)
    {
        if (isDisposing)
            throw new ObjectDisposedException(GetObjectName(disposable), "The object is being disposed.");
    }
    
    public static void ThrowIfDisposed(this IDisposable disposable, bool isDisposed)
    {
        if (isDisposed)
            throw new ObjectDisposedException(GetObjectName(disposable), "The object is disposed.");
    }
    
    public static void ThrowIfDisposingOrDisposed(this IDisposable disposable, bool isDisposingOrDisposed)
    {
        if (isDisposingOrDisposed)
            throw new ObjectDisposedException(GetObjectName(disposable), "The object is being disposed or disposed.");
    }

    private static string? GetObjectName(object obj) => obj.GetType().FullName;
}