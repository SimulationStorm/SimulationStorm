using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateDescriptor(string name, Color color)
{
    public string Name { get; } = name;

    public Color Color { get; } = color;
}