using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules;

public class RuleSetDescriptor<TCellState> where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; }
    
    public int RepetitionCount { get; }

    public IEnumerable<RuleDescriptor<TCellState>> RuleDescriptors { get; }
    
    public RuleSetDescriptor(string name, int repetitionCount, IEnumerable<RuleDescriptor<TCellState>> ruleDescriptors)
    {
        Name = name;
        
        RuleSet<TCellState>.ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleDescriptors = ruleDescriptors;
    }
}