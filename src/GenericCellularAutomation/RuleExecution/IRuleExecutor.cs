using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutor
{
    byte CalculateNextCellState(byte[,] world, Point cellPosition);
}