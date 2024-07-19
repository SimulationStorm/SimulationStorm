using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public interface IGenericCellularAutomationFactory
{
    IGenericCellularAutomation CreateGenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        Size worldSize,
        int maxCellNeighborhoodRadius,
        WorldWrapping worldWrapping,
        CellStateCollection possibleCellStateCollection,
        RuleSetCollection ruleSetCollection
    );
}