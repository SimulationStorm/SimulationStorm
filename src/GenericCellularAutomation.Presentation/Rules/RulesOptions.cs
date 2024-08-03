using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Rules;

public sealed class RulesOptions
{
    public int MaxRuleCount { get; init; }

    public int MaxRuleSetCount { get; init; }

    public Range<int> RuleSetRepetitionCountRange { get; init; }
    
    public Range<int> RuleSetCollectionRepetitionCountRange { get; init; }
}