using SimulationStorm.GameOfLife.DataTypes;

namespace SimulationStorm.GameOfLife.Presentation.Patterns;

public class NamedPattern(string name, GameOfLifePattern pattern)
{
    public string Name { get; } = name;
    
    public GameOfLifePattern Pattern { get; } = pattern;
}