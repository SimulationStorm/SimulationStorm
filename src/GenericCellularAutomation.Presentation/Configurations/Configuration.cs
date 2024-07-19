using System.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules;
using GenericCellularAutomation.Presentation.Rules.Descriptors;

namespace GenericCellularAutomation.Presentation.Configurations;

public sealed class Configuration
(
    string name,
    CellStateCollectionDescriptor cellStateCollectionDescriptor,
    RuleSetCollectionDescriptor ruleSetCollectionDescriptor,
    IEnumerable<PatternDescriptorCategory> patternDescriptorCategories)
{
    public string Name { get; } = name;

    public CellStateCollectionDescriptor CellStateCollectionDescriptor { get; } =
        cellStateCollectionDescriptor;
        
    public RuleSetCollectionDescriptor RuleSetCollectionDescriptor { get; } =
        ruleSetCollectionDescriptor;

    public IEnumerable<PatternDescriptorCategory> PatternDescriptorCategories { get; } =
        patternDescriptorCategories;
}