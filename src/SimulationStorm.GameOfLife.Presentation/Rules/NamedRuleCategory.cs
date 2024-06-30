using System.Collections.Generic;

namespace SimulationStorm.GameOfLife.Presentation.Rules;

public class NamedRuleCategory(string name, IEnumerable<NamedRule> rules)
{
    public string Name { get; } = name;

    public IEnumerable<NamedRule> Rules { get; } = rules;
}