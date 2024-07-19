using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation;

public sealed class GenericCellularAutomationOptions
{
    public Range<int> CellStateNameLengthRange { get; init; }
    
    public int MaxCellStateCount { get; init; }

    public int MaxRuleCountInRuleSet { get; init; }

    public int MaxRuleSetCount { get; init; }

    public int MaxRuleSetRepetitionCount { get; init; }
    
    public int MaxRuleSetCollectionRepetitionCount { get; init; }
}