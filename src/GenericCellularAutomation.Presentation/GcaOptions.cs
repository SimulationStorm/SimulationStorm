using GenericCellularAutomation.Presentation.Configurations;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation.Presentation;

// Todo: GcaOptions or GcaManagerOptions
public sealed class GcaOptions : IBoundedSimulationManagerOptions
{
    public Size WorldSize { get; init; }

    public Range<Size> WorldSizeRange { get; init; }
    
    public WorldWrapping WorldWrapping { get; init; }

    public byte MaxCellState { get; init; }
    
    public int MaxCellNeighborhoodRadius { get; init; }
    
    public Configuration Configuration { get; init; } = null!;
}