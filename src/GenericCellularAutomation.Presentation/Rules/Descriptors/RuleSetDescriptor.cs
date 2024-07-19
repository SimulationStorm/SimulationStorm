using System.Collections.Generic;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetDescriptor
{
    #region Properties
    public string Name { get; }
    
    public int RepetitionCount { get; }

    public IEnumerable<RuleDescriptor> RuleDescriptors { get; }
    #endregion
    
    public RuleSetDescriptor(string name, int repetitionCount, IEnumerable<RuleDescriptor> ruleDescriptors)
    {
        Name = name;
        
        RuleSet.ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleDescriptors = ruleDescriptors;
    }
}