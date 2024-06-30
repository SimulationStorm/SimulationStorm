using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class QueuedSimulationCommand(SimulationCommand command)
{
    #region Properties
    public SimulationCommand Command { get; } = command;

    public Task Task => _completionSource.Task;
    #endregion

    private readonly TaskCompletionSource _completionSource = new();
    
    public void NotifyExecuted() => _completionSource.SetResult();
}