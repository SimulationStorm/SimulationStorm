﻿using System.Collections.Generic;

namespace GenericCellularAutomation.Presentation.Patterns;

public sealed class PatternDescriptorCategory(string name, IEnumerable<PatternDescriptor> patterns)
{
    public string Name { get; } = name;

    public IEnumerable<PatternDescriptor> Patterns { get; } = patterns;
}