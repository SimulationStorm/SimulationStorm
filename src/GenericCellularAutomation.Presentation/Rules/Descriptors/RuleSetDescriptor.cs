using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetDescriptor(string name, int repetitionCount, IEnumerable<RuleDescriptor> rules)
{
    public string Name { get; } = name;

    public int RepetitionCount { get; } = repetitionCount;

    public IEnumerable<RuleDescriptor> Rules { get; } = rules;
}