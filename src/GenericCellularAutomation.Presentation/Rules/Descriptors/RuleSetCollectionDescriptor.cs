using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetCollectionDescriptor
(
    int repetitionCount,
    IEnumerable<RuleSetDescriptor> ruleSets)
{
    public int RepetitionCount { get; } = repetitionCount;

    public IEnumerable<RuleSetDescriptor> RuleSets { get; } = ruleSets;
}