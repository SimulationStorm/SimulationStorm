using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Patterns;

public sealed class PatternDescriptorCategoryBuilder : IFluentBuilder<PatternDescriptorCategory>
{
    #region Fields
    private string _name = string.Empty;

    private readonly ICollection<PatternDescriptor> _patterns = new Collection<PatternDescriptor>();
    #endregion
    
    #region Methods
    public PatternDescriptorCategoryBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public PatternDescriptorCategoryBuilder HasPatterns(params PatternDescriptor[] patterns)
    {
        _patterns.AddAll(patterns);
        return this;
    }

    public PatternDescriptorCategory Build() => new(_name, _patterns);
    #endregion
}