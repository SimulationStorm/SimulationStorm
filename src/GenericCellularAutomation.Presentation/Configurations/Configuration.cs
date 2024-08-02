using System.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules.Descriptors;

namespace GenericCellularAutomation.Presentation.Configurations;

public sealed class Configuration
(
    string name,
    CellStateCollectionDescriptor cellStateCollection,
    RuleSetCollectionDescriptor ruleSetCollection,
    IEnumerable<PatternDescriptorCategory> patternCategories)
{
    public string Name { get; } = name;

    public CellStateCollectionDescriptor CellStateCollection { get; } =
        cellStateCollection;
        
    public RuleSetCollectionDescriptor RuleSetCollection { get; } =
        ruleSetCollection;

    public IEnumerable<PatternDescriptorCategory> PatternCategories { get; } =
        patternCategories;
}