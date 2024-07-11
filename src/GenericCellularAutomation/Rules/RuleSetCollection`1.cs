using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSetCollection<TCellState>(int repetitionCount, IEnumerable<RuleSet<TCellState>> ruleSets)
    where TCellState :
        IComparable,
        IComparable<TCellState>,
        IEquatable<TCellState>,
        IBinaryInteger<TCellState>,
        IMinMaxValue<TCellState>
{
    /// <summary>
    /// Gets how many times <see cref="RuleSets"/> in <see cref="RuleSetCollection{TCellState}"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; } = repetitionCount >= 0 ? repetitionCount
        : throw new ArgumentOutOfRangeException(nameof(repetitionCount));

    public IEnumerable<RuleSet<TCellState>> RuleSets { get; } = ruleSets;
}