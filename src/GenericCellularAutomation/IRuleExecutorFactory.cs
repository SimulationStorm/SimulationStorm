using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation;

public interface IRuleExecutorFactory
{
    IRuleExecutor<TCellState> CreateRuleExecutor<TCellState>(RuleExecutorType type, Rule<TCellState> rule)
        where TCellState : notnull;
}