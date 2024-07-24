using System.Collections.Generic;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetCollectionDescriptorBuilder : IFluentBuilder<RuleSetCollectionDescriptor>
{
    #region Fields
    private int _repetitionCount;

    private readonly IList<RuleSetDescriptor> _ruleSets = new List<RuleSetDescriptor>();
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

    public RuleSetCollectionDescriptor Build() => new(_repetitionCount, (IReadOnlyList<RuleSetDescriptor>)_ruleSets);
    #endregion
}