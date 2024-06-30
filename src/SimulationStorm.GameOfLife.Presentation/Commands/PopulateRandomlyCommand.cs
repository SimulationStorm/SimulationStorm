using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.GameOfLife.Presentation.Commands;

public class PopulateRandomlyCommand(double cellBrithProbability) : SimulationCommand("PopulateRandomly", true)
{
    public double CellBirthProbability { get; } = cellBrithProbability;
}