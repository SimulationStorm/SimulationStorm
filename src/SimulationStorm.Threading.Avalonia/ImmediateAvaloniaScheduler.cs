using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

namespace SimulationStorm.Threading.Avalonia;

/// <summary>
/// A version of <see cref="AvaloniaScheduler"/> that immediately executes scheduled action via <see cref="Dispatcher"/>'s Invoke method.
/// </summary>
public class ImmediateAvaloniaScheduler : LocalScheduler
{
    /// <summary>
    /// Users can schedule actions on the dispatcher thread while being on the correct thread already.
    /// We are optimizing this case by invoking user callback immediately which can lead to stack overflows in certain cases.
    /// To prevent this we are limiting amount of reentrant calls to <see cref="Schedule{TState}"/> before we will
    /// schedule on a dispatcher anyway.
    /// </summary>
    private const int MaxReentrantSchedules = 32;

    private int _reentrancyGuard;

    /// <summary>
    /// The instance of the <see cref="ImmediateAvaloniaScheduler"/>.
    /// </summary>
    public static readonly ImmediateAvaloniaScheduler Instance = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ImmediateAvaloniaScheduler"/> class.
    /// </summary>
    private ImmediateAvaloniaScheduler() { }

    /// <inheritdoc/>
    public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        if (dueTime != TimeSpan.Zero)
        {
            var disposables = new CompositeDisposable(2);
            disposables.Add(DispatcherTimer.RunOnce(() => disposables.Add(action(this, state)), dueTime));
            return disposables;
        }
        
        if (!Dispatcher.UIThread.CheckAccess())
            return InvokeOnDispatcher(action, state);
        
        if (_reentrancyGuard >= MaxReentrantSchedules)
            return InvokeOnDispatcher(action, state);

        try
        {
            _reentrancyGuard++;
            return action(this, state);
        }
        finally
        {
            _reentrancyGuard--;
        }
    }

    private IDisposable InvokeOnDispatcher<TState>(Func<IScheduler, TState, IDisposable> action, TState state)
    {
        var disposables = new CompositeDisposable(2);
        var cancellationSource = new CancellationDisposable();

        Dispatcher.UIThread.Invoke(() =>
        {
            if (!cancellationSource.Token.IsCancellationRequested)
                disposables.Add(action(this, state));
        });

        disposables.Add(cancellationSource);

        return disposables;
    }
}