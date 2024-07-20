using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.CellStates;

public sealed class CellStatesOptions
{
    public Range<int> CellStateNameLengthRange { get; init; }
    
    /// <summary>
    /// Must be less than 255...
    /// </summary>
    public int MaxCellStateCount { get; init; }
}