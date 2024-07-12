using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Presentation.CellStates;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules;

namespace GenericCellularAutomation.Presentation.Configurations;

public class Configuration<TCellState>
(
    string name,
    CellStateCollectionDescriptor<TCellState> cellStateCollectionDescriptor,
    RuleSetCollectionDescriptor<TCellState> ruleSetCollectionDescriptor,
    IEnumerable<PatternDescriptorCategory<TCellState>> patternDescriptorCategories
)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public CellStateCollectionDescriptor<TCellState> CellStateCollectionDescriptor { get; } =
        cellStateCollectionDescriptor;
        
    public RuleSetCollectionDescriptor<TCellState> RuleSetCollectionDescriptor { get; } =
        ruleSetCollectionDescriptor;

    public IEnumerable<PatternDescriptorCategory<TCellState>> PatternDescriptorCategories { get; } =
        patternDescriptorCategories;
}