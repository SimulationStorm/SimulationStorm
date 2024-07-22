using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutorFactory
{
    IRuleExecutor CreateRuleExecutor(RuleExecutorType type, Rule rule);
}