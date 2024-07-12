using System;
using System.Numerics;
using GenericCellularAutomation.Rules;

namespace GenericCellularAutomation.RuleExecution;

public interface IRuleExecutorFactory
{
    IRuleExecutor<TCellState> CreateRuleExecutor<TCellState>(RuleExecutorType ruleType, Rule<TCellState> rule)
        where TCellState :
            IComparable,
            IComparable<TCellState>,
            IEquatable<TCellState>,
            IBinaryInteger<TCellState>,
            IMinMaxValue<TCellState>;
}