using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Presentation.Patterns;

public class PatternDescriptorCategory<TCellState>(string name, IEnumerable<PatternDescriptor<TCellState>> patternDescriptors)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;

    public IEnumerable<PatternDescriptor<TCellState>> PatternDescriptors { get; } = patternDescriptors;
}