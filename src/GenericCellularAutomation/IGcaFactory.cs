using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public interface IGcaFactory
{
    IGenericCellularAutomation CreateGenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        Size worldSize,
        int maxCellNeighborhoodRadius,
        GcaCellState maxCellState,
        WorldWrapping worldWrapping,
        CellStateCollection cellStateCollection,
        RuleSetCollection ruleSetCollection
    );
}