using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife.Presentation.Management;

public class GameOfLifeManagerOptions : IBoundedSimulationManagerOptions
{
    public int CommandExecutedEventHandlerCount { get; init; }

    public Size WorldSize { get; init; }

    public Range<Size> WorldSizeRange { get; init; }
    
    public GameOfLifeAlgorithm Algorithm { get; init; }
    
    public WorldWrapping WorldWrapping { get; init; }

    public GameOfLifeRule Rule { get; init; } = null!;
}