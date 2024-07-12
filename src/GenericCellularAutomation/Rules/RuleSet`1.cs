using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSet<TCellState> where TCellState : IBinaryInteger<TCellState>
{
    /// <summary>
    /// Gets how many times <see cref="Rules"/> sequence in <see cref="RuleSet{TCellState}"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    public IEnumerable<Rule<TCellState>> Rules { get; }

    public RuleSet(int repetitionCount, IEnumerable<Rule<TCellState>> rules)
    {
        ValidateRepetitionCount(repetitionCount);
        RepetitionCount = repetitionCount;
        
        Rules = rules;
    }
    
    public static void ValidateRepetitionCount(int repetitionCount) =>
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
}