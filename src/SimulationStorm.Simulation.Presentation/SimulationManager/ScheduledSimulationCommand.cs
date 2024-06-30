using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class ScheduledSimulationCommand(SimulationCommand command)
{
    #region Properties
    public SimulationCommand Command { get; } = command;

    public Task Task => _tcs.Task;
    #endregion

    private readonly TaskCompletionSource _tcs = new();
    
    public void NotifyTaskCompleted() => _tcs.SetResult();
}