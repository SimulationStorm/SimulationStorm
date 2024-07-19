namespace GenericCellularAutomation.Presentation.Patterns;

public sealed class PatternDescriptor(string name, Pattern pattern)
{
    public string Name { get; } = name;
    
    public Pattern Pattern { get; } = pattern;
}