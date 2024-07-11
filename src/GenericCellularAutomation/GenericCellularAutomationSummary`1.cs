using System.Collections.Generic;

namespace GenericCellularAutomation;

public class GenericCellularAutomationSummary<TCellState>(IDictionary<TCellState, int> cellCountByStates)
{
    public IDictionary<TCellState, int> CellCountByStates { get; } = cellCountByStates;
}