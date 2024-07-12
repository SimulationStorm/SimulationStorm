using System.Numerics;

namespace GenericCellularAutomation.Presentation.Patterns;

public class PatternDescriptor<TCellState>(string name, Pattern<TCellState> pattern)
    where TCellState : IBinaryInteger<TCellState>
{
    public string Name { get; } = name;
    
    public Pattern<TCellState> Pattern { get; } = pattern;
}