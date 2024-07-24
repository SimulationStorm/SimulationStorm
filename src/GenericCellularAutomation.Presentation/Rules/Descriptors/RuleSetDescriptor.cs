using System.Collections.Generic;
using System.Linq;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetDescriptor(string name, int repetitionCount, IReadOnlyList<RuleDescriptor> rules)
{
    #region Properties
    public string Name { get; } = name;

    public int RepetitionCount { get; } = repetitionCount;

    public IReadOnlyList<RuleDescriptor> Rules { get; } = rules;
    #endregion

    public RuleSet ToRuleSet() => new
    (
        RepetitionCount,
        Rules
            .Select(rd => rd.Rule)
            .ToArray()
    );
}