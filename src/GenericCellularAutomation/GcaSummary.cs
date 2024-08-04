using System.Collections.Generic;

namespace GenericCellularAutomation;

public sealed class GcaSummary(IDictionary<GcaCellState, int> cellCountByStates)
{
    public IDictionary<GcaCellState, int> CellCountByStates { get; } = cellCountByStates;
}