using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateCollectionDescriptor
(
    IEnumerable<CellStateDescriptor> cellStateDescriptors,
    CellStateDescriptor defaultCellStateDescriptor)
{
    public IEnumerable<CellStateDescriptor> CellStateDescriptors { get; } = cellStateDescriptors;

    public CellStateDescriptor DefaultCellStateDescriptor { get; } = defaultCellStateDescriptor;
}