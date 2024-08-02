using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules.Descriptors;

namespace GenericCellularAutomation.Presentation.Configurations;

public sealed class ConfigurationBuilder : IFluentBuilder<Configuration>
{
    #region Fields
    private string _name = string.Empty;

    private CellStateCollectionDescriptor _cellStateCollection = null!;

    private RuleSetCollectionDescriptor _ruleSetCollection = null!;

    private readonly ICollection<PatternDescriptorCategory> _patternCategories =
        new Collection<PatternDescriptorCategory>();
    #endregion

    #region Methods
    public ConfigurationBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public ConfigurationBuilder HasCellStateCollection(CellStateCollectionDescriptor cellStateCollection)
    {
        _cellStateCollection = cellStateCollection;
        return this;
    }
    
    public ConfigurationBuilder HasRuleSetCollection(RuleSetCollectionDescriptor ruleSetCollection)
    {
        _ruleSetCollection = ruleSetCollection;
        return this;
    }

    public ConfigurationBuilder HasPatternCategories(params PatternDescriptorCategory[] patternCategories)
    {
        _patternCategories.AddAll(patternCategories);
        return this;
    }

    public Configuration Build() => new(_name, _cellStateCollection, _ruleSetCollection, _patternCategories);
    #endregion
}