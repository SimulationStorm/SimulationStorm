using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

namespace SimulationStorm.Avalonia.Extensions;

public static class AvaloniaObjectExtensions
{
    public static T GetValueOnUiThread<T>(this AvaloniaObject target, AvaloniaProperty<T> property) =>
        Dispatcher.UIThread.Invoke(() => global::Avalonia.AvaloniaObjectExtensions.GetValue(target, property));
    
    public static IObservable<T> GetObservableOnUiThread<T>
    (
        this AvaloniaObject obj,
        AvaloniaProperty<T> property)
    {
        return obj
            .GetObservable(property)
            .ObserveOn(AvaloniaScheduler.Instance)
            .SubscribeOn(AvaloniaScheduler.Instance);
    }
    
    public static IObservable<object?> GetObservableOnUiThread
    (
        this AvaloniaObject obj,
        AvaloniaProperty property)
    {
        return obj
            .GetObservable(property)
            .ObserveOn(AvaloniaScheduler.Instance)
            .SubscribeOn(AvaloniaScheduler.Instance);
    }
}