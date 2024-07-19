using GenericCellularAutomation.Rules;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace GenericCellularAutomation.Presentation.Management.Commands;

public sealed class ChangeRuleSetCollectionCommand(RuleSetCollection newRuleSetCollection) :
    SimulationCommand("ChangeRuleSetCollection", false)
{
    public RuleSetCollection NewRuleSetCollection { get; } = newRuleSetCollection;
}