using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules;

public class RuleSetCollectionDescriptor<TCellState> where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; }

    public int RepetitionCount { get; }
    
    public IEnumerable<RuleSetDescriptor<TCellState>> RuleSetDescriptors { get; }
    
    public RuleSetCollectionDescriptor(string name, int repetitionCount, IEnumerable<RuleSetDescriptor<TCellState>> ruleSetDescriptors)
    {
        Name = name;
        
        RuleSetCollection<TCellState>.ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleSetDescriptors = ruleSetDescriptors;
    }
}