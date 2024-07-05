using System.Collections.Generic;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife.DataTypes;

public class GameOfLifeSave
(
    Size worldSize,
    WorldWrapping worldWrapping,
    GameOfLifeRule rule,
    GameOfLifeAlgorithm algorithm,
    byte[]? world = null,
    IReadOnlySet<Point>? aliveCells = null)
{
    public Size WorldSize { get; } = worldSize;

    public WorldWrapping WorldWrapping { get; } = worldWrapping;

    public GameOfLifeRule Rule { get; } = rule;
    
    public GameOfLifeAlgorithm Algorithm { get; } = algorithm;

    /// <summary>
    /// Used with <see cref="GameOfLifeAlgorithm.Bitwise"/>.
    /// </summary>
    public byte[]? World { get; } = world;
    
    /// <summary>
    /// Used with <see cref="GameOfLifeAlgorithm.Smart"/>.
    /// </summary>
    public IReadOnlySet<Point>? AliveCells { get; } = aliveCells;
}