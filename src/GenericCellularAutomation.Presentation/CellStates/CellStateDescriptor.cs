using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public sealed class CellStateDescriptor(byte cellState, string name, Color color)
{
    public byte CellState { get; } = cellState;

    public string Name { get; } = name;

    public Color Color { get; } = color;
}