using System.Collections.Generic;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetCollectionDescriptor
{
    #region Properties
    public string Name { get; }

    public int RepetitionCount { get; }
    
    public IEnumerable<RuleSetDescriptor> RuleSetDescriptors { get; }
    #endregion
    
    public RuleSetCollectionDescriptor(string name, int repetitionCount, IEnumerable<RuleSetDescriptor> ruleSetDescriptors)
    {
        Name = name;
        
        RuleSetCollection.ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleSetDescriptors = ruleSetDescriptors;
    }
}