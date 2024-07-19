using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleDescriptor(string name, Rule rule)
{
    public string Name { get; } = name;

    public Rule Rule { get; } = rule;
}