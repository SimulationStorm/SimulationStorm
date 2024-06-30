using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife;

public interface IGameOfLifeFactory
{
    IGameOfLife CreateGameOfLife(GameOfLifeAlgorithm algorithm,
        Size size, WorldWrapping wrapping, GameOfLifeRule rule);
}