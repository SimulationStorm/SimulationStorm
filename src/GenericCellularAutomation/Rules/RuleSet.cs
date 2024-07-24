using System;
using System.Collections.Generic;

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
    public IReadOnlyList<Rule> Rules { get; }
    #endregion
    
    public RuleSet(int repetitionCount, IReadOnlyList<Rule> rules)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
        RepetitionCount = repetitionCount;
        
        Rules = rules;
    }
}