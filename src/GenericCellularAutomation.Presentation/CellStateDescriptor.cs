using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation;

public readonly struct CellStateDescriptor<TCellState>(TCellState number, string name, Color color)
{
    public TCellState Number { get; } = number;

    public string Name { get; } = name;

    public Color Color { get; } = color;
}