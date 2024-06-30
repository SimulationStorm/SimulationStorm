using System.Reactive.Concurrency;

namespace SimulationStorm.Threading.Presentation;

/// <summary>
/// Represents an object that schedules units of work on the user interface thread.
/// </summary>
public interface IUiThreadScheduler : IScheduler;