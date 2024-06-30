using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.GameOfLife.Presentation.Commands;

public class ChangeAlgorithmCommand(GameOfLifeAlgorithm newAlgorithm) : SimulationCommand("ChangeAlgorithm", false)
{
    public GameOfLifeAlgorithm NewAlgorithm { get; } = newAlgorithm;
}