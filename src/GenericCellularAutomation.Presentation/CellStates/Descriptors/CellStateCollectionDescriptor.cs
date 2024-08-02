using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateCollectionDescriptor
(
    IReadOnlyList<CellStateDescriptor> cellStates,
    CellStateDescriptor defaultCellState)
{
    #region Properties
    public IReadOnlyList<CellStateDescriptor> CellStates { get; } = cellStates;

    public CellStateDescriptor DefaultCellState { get; } = defaultCellState;
    #endregion

    public CellStateCollection ToCellStateCollection() => new
    (
        CellStates
            .Select(csd => csd.CellState)
            .ToHashSet(),
        DefaultCellState.CellState
    );
}