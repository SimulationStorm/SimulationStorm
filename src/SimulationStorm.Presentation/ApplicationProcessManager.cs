using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Threading;

namespace SimulationStorm.Presentation;

public static class ApplicationProcessManager
{
    public static bool TryGetLock(string applicationName, [NotNullWhen(true)] out IDisposable? @lock)
    {
        @lock = null;
        
        var mutex = new Mutex(true, applicationName, out var wasMutexCreated);

        if (wasMutexCreated)
            @lock = Disposable.Create(mutex.Dispose);

        return @lock is not null;
    }
}