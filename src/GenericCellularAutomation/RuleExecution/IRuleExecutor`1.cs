using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutor<TCellState>
{
    TCellState CalculateNextCellState(TCellState[,] world, Point cellPosition);
}