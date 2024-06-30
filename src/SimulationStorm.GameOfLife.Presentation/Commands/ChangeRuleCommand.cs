using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.GameOfLife.Presentation.Commands;

public class ChangeRuleCommand(GameOfLifeRule newRule) : SimulationCommand("ChangeRule", false)
{
    public GameOfLifeRule NewRule { get; } = newRule;
}