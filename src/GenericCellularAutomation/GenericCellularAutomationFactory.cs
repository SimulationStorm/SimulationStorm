using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public sealed class GenericCellularAutomationFactory : IGenericCellularAutomationFactory
{
    public IGenericCellularAutomation CreateGenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        Size worldSize,
        int maxCellNeighborhoodRadius,
        WorldWrapping worldWrapping,
        CellStateCollection possibleCellStateCollection,
        RuleSetCollection ruleSetCollection
    )
        => new GenericCellularAutomation(ruleExecutorFactory, worldSize,
            maxCellNeighborhoodRadius, worldWrapping, possibleCellStateCollection, ruleSetCollection);
}