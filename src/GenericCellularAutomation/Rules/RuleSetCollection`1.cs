using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSetCollection<TCellState> where TCellState : IBinaryInteger<TCellState>
{
    /// <summary>
    /// Gets how many times <see cref="RuleSets"/> sequence in <see cref="RuleSetCollection{TCellState}"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    public IEnumerable<RuleSet<TCellState>> RuleSets { get; }
    
    public RuleSetCollection(int repetitionCount, IEnumerable<RuleSet<TCellState>> ruleSets)
    {
        ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        RuleSets = ruleSets;
    }
    
    public static void ValidateRepetitionCount(int repetitionCount) =>
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
}