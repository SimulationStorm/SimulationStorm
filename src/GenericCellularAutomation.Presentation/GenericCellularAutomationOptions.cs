using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation;

public sealed class GenericCellularAutomationOptions
{
    public Range<int> CellStateNameLengthRange { get; init; }
    
    /// <summary>
    /// Must be less than 255...
    /// </summary>
    public int MaxCellStateCount { get; init; }

    public int MaxRuleCountInRuleSet { get; init; }

    public int MaxRuleSetCount { get; init; }

    public int MaxRuleSetRepetitionCount { get; init; }
    
    public int MaxRuleSetCollectionRepetitionCount { get; init; }
}