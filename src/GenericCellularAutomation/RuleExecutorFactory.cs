using System;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation;

public class RuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor<TCellState> CreateRuleExecutor<TCellState>(RuleExecutorType type, Rule<TCellState> rule)
        where TCellState : notnull => rule switch
    {
        RuleExecutorType.Straightforward => new StraightforwardRuleExecutor<TCellState>(rule),
        _ => throw new NotImplementedException()
    };
}