using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules;

public class RuleDescriptor<TCellState>(string name, Rule<TCellState> rule)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public Rule<TCellState> Rule { get; } = rule;
}