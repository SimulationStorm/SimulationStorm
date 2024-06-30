using System.Collections.Generic;

namespace SimulationStorm.GameOfLife.Presentation.Patterns;

public class NamedPatternCategory(string name, IEnumerable<NamedPattern> patterns)
{
    public string Name { get; } = name;

    public IEnumerable<NamedPattern> Patterns { get; } = patterns;
}