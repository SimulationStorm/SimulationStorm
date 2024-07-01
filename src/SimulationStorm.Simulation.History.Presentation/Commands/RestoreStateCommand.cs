using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.History.Presentation.Commands;

public class RestoreStateCommand(object state, bool isRestoringFromAppSave = false) : SimulationCommand("RestoreState", true)
{
    public object State { get; } = state;

    public bool IsRestoringFromAppSave { get; } = isRestoringFromAppSave;
}