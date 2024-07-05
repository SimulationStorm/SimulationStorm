using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.History.Presentation.Commands;

public class RestoreSaveCommand(object save, bool isRestoringFromAppSave = false) : SimulationCommand("RestoreSave", true)
{
    public object Save { get; } = save;

    public bool IsRestoringFromAppSave { get; } = isRestoringFromAppSave;
}