namespace GenericCellularAutomation.Presentation.Patterns;

public sealed class PatternDescriptor(string name, GcaPattern gcaPattern)
{
    public string Name { get; } = name;

    public GcaPattern GcaPattern { get; } = gcaPattern;
}