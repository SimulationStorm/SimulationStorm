using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace SimulationStorm.Utilities;

public static class TaskExtensions
{
    // [NOTE] If original task will not be returned, then it can not be awaited.
    public static Task ThrowWhenFaulted(this Task task)
    {
        task.ContinueWith(previousTask =>
        {
            if (previousTask is { IsFaulted: true })
                ExceptionDispatchInfo.Capture(previousTask.Exception.InnerException ?? previousTask.Exception).Throw();
        },
            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

        return task;
    }
}