using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace SimulationStorm.Exceptions;

public class TaskSchedulerExceptionBubbler : IDisposable
{
    public TaskSchedulerExceptionBubbler() =>
        TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;

    private static void OnTaskSchedulerUnobservedTaskException(object? _, UnobservedTaskExceptionEventArgs e) =>
        ExceptionDispatchInfo.Capture(e.Exception.InnerException ?? e.Exception).Throw();

    public void Dispose()
    {
        TaskScheduler.UnobservedTaskException -= OnTaskSchedulerUnobservedTaskException;
        GC.SuppressFinalize(this);
    }
}