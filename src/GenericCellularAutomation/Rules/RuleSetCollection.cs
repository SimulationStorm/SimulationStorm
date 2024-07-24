using System;
using System.Collections.Generic;
using DotNext.Collections.Generic;

namespace GenericCellularAutomation.Rules;

public sealed class RuleSetCollection
{
    #region Properties
    /// <summary>
    /// Gets how many times <see cref="RuleSets"/> sequence in <see cref="RuleSetCollection"/> should be executed.
    /// </summary>
    public int RepetitionCount { get; }

    /// <summary>
    /// Gets rule sets included in the rule set collection.
    /// </summary>
    public IReadOnlyList<RuleSet> RuleSets { get; }

    /// <summary>
    /// Gets cell states used in rules in the rule sets.
    /// </summary>
    public IReadOnlySet<byte> UsedCellStates { get; }
    #endregion
    
    public RuleSetCollection(int repetitionCount, IReadOnlyList<RuleSet> ruleSets)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(repetitionCount, nameof(repetitionCount));
        RepetitionCount = repetitionCount;
        
        RuleSets = ruleSets;
        UsedCellStates = GetCellStatesUsedInRuleSets(RuleSets);
    }

    private static IReadOnlySet<byte> GetCellStatesUsedInRuleSets(IEnumerable<RuleSet> ruleSets)
    {
        var usedCellStates = new HashSet<byte>();
        
        ruleSets.ForEach(ruleSet =>
            ruleSet.Rules.ForEach(rule =>
            {
                usedCellStates.Add(rule.TargetCellState);
                usedCellStates.Add(rule.NewCellState);

                if (rule.NeighborCellState is { } neighborCellState)
                    usedCellStates.Add(neighborCellState);
            })
        );

        return usedCellStates;
    }
}