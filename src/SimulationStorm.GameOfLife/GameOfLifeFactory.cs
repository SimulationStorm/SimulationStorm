using System.ComponentModel;
using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife;

public class GameOfLifeFactory : IGameOfLifeFactory
{
    public IGameOfLife CreateGameOfLife(GameOfLifeAlgorithm algorithm, Size size, WorldWrapping wrapping, GameOfLifeRule rule) => algorithm switch
    {
        GameOfLifeAlgorithm.Smart => new SmartGameOfLife(size, wrapping, rule),
        GameOfLifeAlgorithm.Bitwise => new BitwiseGameOfLife(size, wrapping, rule),
        GameOfLifeAlgorithm.Vector => new VectorGameOfLife(size, wrapping, rule),
        _ => throw new InvalidEnumArgumentException(nameof(algorithm), (int)algorithm, typeof(GameOfLifeAlgorithm))
    };
}