using System;
using System.Reactive.Concurrency;
using Avalonia.ReactiveUI;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Threading.Avalonia;

/// <inheritdoc cref="IUiThreadScheduler"/>
public class UiThreadScheduler : LocalScheduler, IUiThreadScheduler
{
    /// <inheritdoc/>
    public override IDisposable Schedule<TState>
    (
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
    {
        return AvaloniaScheduler.Instance.Schedule(state, dueTime, action);
    }
}