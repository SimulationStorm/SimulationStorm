using System;
using System.Reactive.Concurrency;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Threading.Avalonia;

/// <inheritdoc cref="IImmediateUiThreadScheduler"/>
public class ImmediateUiThreadScheduler : LocalScheduler, IImmediateUiThreadScheduler
{
    /// <inheritdoc/>
    public override IDisposable Schedule<TState>
    (
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
    {
        return ImmediateAvaloniaScheduler.Instance.Schedule(state, dueTime, action);
    }
}