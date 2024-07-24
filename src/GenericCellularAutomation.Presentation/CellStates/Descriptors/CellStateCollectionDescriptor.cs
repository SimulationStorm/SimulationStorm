using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateCollectionDescriptor
(
    IReadOnlyList<CellStateDescriptor> cellStates,
    CellStateDescriptor defaultCellState)
{
    public IReadOnlyList<CellStateDescriptor> CellStates { get; } = cellStates;

    public CellStateDescriptor DefaultCellState { get; } = defaultCellState;

    public CellStateCollection ToCellStateCollection() => new
    (
        CellStates
            .Select(csd => csd.CellState)
            .ToHashSet(),
        DefaultCellState.CellState
    );
}