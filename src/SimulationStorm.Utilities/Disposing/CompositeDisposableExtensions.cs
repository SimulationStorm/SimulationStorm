using System;
using System.Reactive.Disposables;

namespace SimulationStorm.Utilities.Disposing;

public static class CompositeDisposableExtensions
{
    public static void AddRange(this CompositeDisposable disposables, params IDisposable[] items)
    {
        foreach (var item in items)
            disposables.Add(item);
    }
}