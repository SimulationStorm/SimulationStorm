using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutorFactory
{
    IRuleExecutor CreateRuleExecutor(Rule rule);
}