using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation;

public class GenericCellularAutomationSummary<TCellState>(IDictionary<TCellState, int> cellCountByStates)
    where TCellState : IBinaryInteger<TCellState>
{
    public IDictionary<TCellState, int> CellCountByStates { get; } = cellCountByStates;
}