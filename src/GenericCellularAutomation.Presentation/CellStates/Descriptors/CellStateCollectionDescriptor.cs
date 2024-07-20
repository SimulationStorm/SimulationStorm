using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateCollectionDescriptor
(
    IEnumerable<CellStateDescriptor> cellStateDescriptors,
    CellStateDescriptor defaultCellStateDescriptor)
{
    public IEnumerable<CellStateDescriptor> CellStateDescriptors { get; } = cellStateDescriptors;

    public CellStateDescriptor DefaultCellStateDescriptor { get; } = defaultCellStateDescriptor;

    public CellStateCollection ToCellStateCollection() => new
    (
        CellStateDescriptors
            .Select(csd => csd.CellState)
            .ToHashSet(),
        DefaultCellStateDescriptor.CellState
    );
}