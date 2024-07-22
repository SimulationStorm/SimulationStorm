using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public sealed class StraightforwardRuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor CreateRuleExecutor(Rule rule) => new StraightforwardRuleExecutor(rule);
}