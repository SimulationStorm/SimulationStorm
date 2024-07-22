using System.ComponentModel;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public sealed class RuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor CreateRuleExecutor(RuleExecutorType type, Rule rule) => type switch
    {
        RuleExecutorType.Straightforward => new StraightforwardRuleExecutor(rule),
        RuleExecutorType.Compiled => new CompiledRuleExecutor(rule),
        _ => throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(RuleExecutorType))
    };
}