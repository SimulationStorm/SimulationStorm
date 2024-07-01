using System.Threading.Tasks;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public abstract partial class SimulationManagerBase
{
    private class ScheduledCommand(SimulationCommand command)
    {
        #region Properties
        public SimulationCommand Command { get; } = command;

        public Task Task => _tcs.Task;
        #endregion

        private readonly TaskCompletionSource _tcs = new();

        #region Methods
        public void NotifyCompleted() => _tcs.SetResult();

        public void NotifyCanceled() => _tcs.SetCanceled();
        #endregion
    }
}