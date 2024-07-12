using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Presentation.Patterns;

namespace GenericCellularAutomation.Presentation.Configurations;

public class Configuration<TCellState>
(
    string name,
    IEnumerable<PatternDescriptorCategory<TCellState>> patternDescriptorCategories)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public IEnumerable<PatternDescriptorCategory<TCellState>> PatternDescriptorCategories { get; } =
        patternDescriptorCategories;
}