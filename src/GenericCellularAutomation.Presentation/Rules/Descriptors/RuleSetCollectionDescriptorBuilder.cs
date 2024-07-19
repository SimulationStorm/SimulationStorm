using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetCollectionDescriptorBuilder : IFluentBuilder<RuleSetCollectionDescriptor>
{
    #region Fields
    private int _repetitionCount;

    private readonly ICollection<RuleSetDescriptor> _ruleSets = new Collection<RuleSetDescriptor>();
    #endregion

    #region Methods
    public RuleSetCollectionDescriptorBuilder HasRepetitionCount(int repetitionCount)
    {
        _repetitionCount = repetitionCount;
        return this;
    }
    
    public RuleSetCollectionDescriptorBuilder HasRuleSets(params RuleSetDescriptor[] ruleSets)
    {
        _ruleSets.AddAll(ruleSets);
        return this;
    }

    public RuleSetCollectionDescriptor Build() => new(_repetitionCount, _ruleSets);
    #endregion
}