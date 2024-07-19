using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSetCollection
{
    /// <summary>
    /// Gets how many times <see cref="RuleSets"/> sequence in <see cref="RuleSetCollection"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    public IEnumerable<RuleSet> RuleSets { get; }
    
    public RuleSetCollection(int repetitionCount, IEnumerable<RuleSet> ruleSets)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
        RepetitionCount = repetitionCount;
        
        RuleSets = ruleSets;
    }

    public RuleSetCollection WithRuleSet(RuleSet ruleSet)
    {
        if (RuleSets.Contains(ruleSet))
            throw new ArgumentException("The rule set is already in the collection.", nameof(ruleSet));

        return new RuleSetCollection(RepetitionCount, RuleSets.Append(ruleSet));
    }
    
    public RuleSetCollection WithoutRuleSet(RuleSet ruleSet)
    {
        if (!RuleSets.Contains(ruleSet))
            throw new ArgumentException("The rule set is not in the collection.", nameof(ruleSet));

        return new RuleSetCollection(RepetitionCount, RuleSets.Where(rs => rs != ruleSet));
    }
}