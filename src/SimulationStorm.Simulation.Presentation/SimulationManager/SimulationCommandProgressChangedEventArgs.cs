namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public class SimulationCommandProgressChangedEventArgs(SimulationCommand command, int progressPercentage) :
    SimulationCommandEventArgs(command)
{
    public int ProgressPercentage { get; } = progressPercentage;
}