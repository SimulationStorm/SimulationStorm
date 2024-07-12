using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSet<TCellState>(int repetitionCount, IEnumerable<Rule<TCellState>> rules)
    where TCellState : IBinaryInteger<TCellState>
{
    /// <summary>
    /// Gets how many times <see cref="Rules"/> collection in <see cref="RuleSet{TCellState}"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; } = repetitionCount >= 0 ? repetitionCount
        : throw new ArgumentOutOfRangeException(nameof(repetitionCount));

    public IEnumerable<Rule<TCellState>> Rules { get; } = rules;
}