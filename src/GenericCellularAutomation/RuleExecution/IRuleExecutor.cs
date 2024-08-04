using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutor
{
    GcaCellState CalculateNextCellState(GcaCellState[,] world, Point cellPosition);
}