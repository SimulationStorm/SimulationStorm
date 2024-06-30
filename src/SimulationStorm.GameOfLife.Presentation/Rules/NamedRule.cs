using SimulationStorm.GameOfLife.DataTypes;

namespace SimulationStorm.GameOfLife.Presentation.Rules;

public class NamedRule(string name, GameOfLifeRule rule)
{
    public string Name { get; } = name;

    public GameOfLifeRule Rule { get; } = rule;
}