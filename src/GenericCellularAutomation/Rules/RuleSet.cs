﻿using System;
using System.Collections.Generic;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSet
{
    /// <summary>
    /// Gets how many times <see cref="Rules"/> sequence in <see cref="RuleSet"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    public IEnumerable<Rule> Rules { get; }

    public RuleSet(int repetitionCount, IEnumerable<Rule> rules)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
        RepetitionCount = repetitionCount;
        
        Rules = rules;
    }
}