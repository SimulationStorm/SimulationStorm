using System.Collections.Generic;

namespace GenericCellularAutomation;

public sealed class GcaSummary(IDictionary<byte, int> cellCountByStates)
{
    public IDictionary<byte, int> CellCountByStates { get; } = cellCountByStates;
}