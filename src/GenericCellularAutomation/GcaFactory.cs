using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public sealed class GcaFactory : IGcaFactory
{
    public IGenericCellularAutomation CreateGenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        Size worldSize,
        int maxCellNeighborhoodRadius,
        byte maxCellState,
        WorldWrapping worldWrapping,
        CellStateCollection cellStateCollection,
        RuleSetCollection ruleSetCollection)
    {
        return new GenericCellularAutomation(ruleExecutorFactory, worldSize,
            maxCellNeighborhoodRadius, maxCellState, worldWrapping, cellStateCollection, ruleSetCollection);
    }
}