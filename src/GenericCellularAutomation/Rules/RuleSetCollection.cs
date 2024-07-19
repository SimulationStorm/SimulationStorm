using System;
using System.Collections.Generic;

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
        ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleSets = ruleSets;
    }
    
    public static void ValidateRepetitionCount(int repetitionCount) =>
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
}