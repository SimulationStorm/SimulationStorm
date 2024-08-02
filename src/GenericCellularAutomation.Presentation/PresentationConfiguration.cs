using GenericCellularAutomation.Presentation.Configurations;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation.Presentation;

public static class PresentationConfiguration
{
    public static readonly GcaOptions GcaOptions = new()
    {
        WorldSize = new Size(192, 108),
        WorldSizeRange = new Range<Size>(new Size(1, 1), new Size(1_920, 1_080)),
        WorldWrapping = WorldWrapping.NoWrap,
        MaxCellNeighborhoodRadius = 5,
        Configuration = PredefinedConfigurations.GameOfLife
    };
}