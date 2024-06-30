using System;
using System.Threading.Tasks;
using SimulationStorm.Exceptions.Unhandled;
using UnhandledExceptionEventArgs = SimulationStorm.Exceptions.Unhandled.UnhandledExceptionEventArgs;

namespace SimulationStorm.Exceptions.Notifiers;

public class TaskSchedulerExceptionNotifier : INotifyUnhandledException, IDisposable
{
    public event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;

    public TaskSchedulerExceptionNotifier() => TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

    private void OnUnobservedTaskException(object? _, UnobservedTaskExceptionEventArgs e)
    {
        var eventArgs = new UnhandledExceptionEventArgs(e.Exception, true);
        UnhandledException?.Invoke(this, eventArgs);

        if (eventArgs.Handled)
            e.SetObserved();
    }

    public void Dispose()
    {
        TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
        GC.SuppressFinalize(this);
    }
}