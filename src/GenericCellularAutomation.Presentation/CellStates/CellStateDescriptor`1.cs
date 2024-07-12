using System.Numerics;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public class CellStateDescriptor<TCellState>(TCellState number, string name, Color color)
    where TCellState : IBinaryInteger<TCellState>
{
    public TCellState Number { get; } = number;

    public string Name { get; } = name;

    public Color Color { get; } = color;
}