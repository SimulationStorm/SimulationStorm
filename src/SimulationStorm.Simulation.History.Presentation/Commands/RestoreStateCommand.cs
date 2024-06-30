using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.History.Presentation.Commands;

public class RestoreStateCommand(object state, bool isRestoringFromAppState = false) : SimulationCommand("RestoreState", true)
{
    public object State { get; } = state;

    public bool IsRestoringFromAppState { get; } = isRestoringFromAppState;
}