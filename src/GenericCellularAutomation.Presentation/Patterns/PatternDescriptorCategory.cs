using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.Patterns;

public class PatternDescriptorCategory(string name, IEnumerable<PatternDescriptor> patternDescriptors)
{
    public string Name { get; } = name;

    public IEnumerable<PatternDescriptor> PatternDescriptors { get; } = patternDescriptors;
}