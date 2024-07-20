using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSet
{
    #region Properties
    /// <summary>
    /// Gets how many times <see cref="Rules"/> sequence in <see cref="RuleSet"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    /// <summary>
    /// Gets the rules included in the set.
    /// </summary>
    public IEnumerable<Rule> Rules { get; }
    #endregion
    
    public RuleSet(int repetitionCount, IEnumerable<Rule> rules)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
        RepetitionCount = repetitionCount;
        
        Rules = rules;
    }
    
    public RuleSet WithRule(Rule rule)
    {
        if (Rules.Contains(rule))
            throw new ArgumentException("The rule is already in the set.", nameof(rule));

        return new RuleSet(RepetitionCount, Rules.Append(rule));
    }
    
    public RuleSet WithoutRule(Rule rule)
    {
        if (!Rules.Contains(rule))
            throw new ArgumentException("The rule is not in the set.", nameof(rule));
        
        return new RuleSet(RepetitionCount, Rules.Where(r => r != rule));
    }
}