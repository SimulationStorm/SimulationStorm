using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.CellStates;

public sealed class CellStateCollectionDescriptor
(
    string name,
    IEnumerable<CellStateDescriptor> cellStateDescriptors,
    CellStateDescriptor defaultCellStateDescriptor)
{
    public string Name { get; } = name;

    public IEnumerable<CellStateDescriptor> CellStateDescriptors { get; } = cellStateDescriptors;

    public CellStateDescriptor DefaultCellStateDescriptor { get; } = defaultCellStateDescriptor;
}