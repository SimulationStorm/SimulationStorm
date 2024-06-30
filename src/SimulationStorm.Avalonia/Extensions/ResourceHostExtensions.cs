using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

namespace SimulationStorm.Avalonia.Extensions;

public static class ResourceHostExtensions
{
    public static IObservable<object?> GetResourceObservableOnUiThread
    (
        this IResourceHost resourceHost,
        object key,
        Func<object?, object?>? converter = null)
    {
        return resourceHost
            .GetResourceObservable(key, converter)
            .ObserveOn(AvaloniaScheduler.Instance)
            .SubscribeOn(AvaloniaScheduler.Instance);
    }
}