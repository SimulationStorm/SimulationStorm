using System;
using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public class RuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor<TCellState> CreateRuleExecutor<TCellState>(RuleExecutorType ruleType, Rule<TCellState> rule)
        where TCellState :
            IComparable,
            IComparable<TCellState>,
            IEquatable<TCellState>,
            IBinaryInteger<TCellState>,
            IMinMaxValue<TCellState>
        => ruleType switch
    {
        RuleExecutorType.Straightforward => new StraightforwardRuleExecutor<TCellState>(rule),
        _ => throw new NotImplementedException()
    };
}