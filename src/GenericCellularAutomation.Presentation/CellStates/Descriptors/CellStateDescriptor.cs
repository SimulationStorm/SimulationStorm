using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateDescriptor(GcaCellState cellState, string name, Color color)
{
    public GcaCellState CellState { get; } = cellState;

    public string Name { get; } = name;

    public Color Color { get; } = color;
}