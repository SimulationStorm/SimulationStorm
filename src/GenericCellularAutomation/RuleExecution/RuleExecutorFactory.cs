using System;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public sealed class RuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor CreateRuleExecutor(RuleExecutorType ruleType, Rule rule) => ruleType switch
    {
        RuleExecutorType.Straightforward => new StraightforwardRuleExecutor(rule),
        _ => throw new NotSupportedException()
    };
}