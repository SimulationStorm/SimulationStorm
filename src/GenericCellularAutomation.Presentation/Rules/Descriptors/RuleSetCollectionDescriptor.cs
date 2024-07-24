using System.Collections.Generic;
using System.Linq;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetCollectionDescriptor
(
    int repetitionCount,
    IReadOnlyList<RuleSetDescriptor> ruleSets)
{
    #region Properties
    public int RepetitionCount { get; } = repetitionCount;

    public IReadOnlyList<RuleSetDescriptor> RuleSets { get; } = ruleSets;
    #endregion

    public RuleSetCollection ToRuleSetCollection() => new
    (
        RepetitionCount,
        RuleSets
            .Select(rsd => rsd.ToRuleSet())
            .ToArray()
    );
}