using System.Collections.Generic;

namespace GenericCellularAutomation;

public sealed class GenericCellularAutomationSummary(IDictionary<byte, int> cellCountByStates)
{
    public IDictionary<byte, int> CellCountByStates { get; } = cellCountByStates;
}