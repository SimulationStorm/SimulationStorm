using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Presentation.CellStates;

public class CellStateCollectionDescriptor<TCellState>
(
    string name,
    IEnumerable<CellStateDescriptor<TCellState>> cellStateDescriptors,
    CellStateDescriptor<TCellState> defaultCellStateDescriptor
)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public IEnumerable<CellStateDescriptor<TCellState>> CellStateDescriptors { get; } = cellStateDescriptors;

    public CellStateDescriptor<TCellState> DefaultCellStateDescriptor { get; } = defaultCellStateDescriptor;
}