using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules;

public class RuleDescriptorCategory<TCellState>(string name, IEnumerable<Rule<TCellState>> ruleDescriptors)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public IEnumerable<Rule<TCellState>> RuleDescriptors { get; } = ruleDescriptors;
}