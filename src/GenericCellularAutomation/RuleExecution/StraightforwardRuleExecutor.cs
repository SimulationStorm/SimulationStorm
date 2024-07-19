using System;
using System.Linq;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

// Todo: rule executor base
public sealed class StraightforwardRuleExecutor : IRuleExecutor
{
    #region Fields
    private readonly Rule _rule;
    private readonly TotalisticRule? _totalisticRule;
    private readonly NontotalisticRule? _nontotalisticRule;
    
    private readonly NextCellStateCalculator _nextCellStateCalculator;
    #endregion

    public StraightforwardRuleExecutor(Rule rule)
    {
        _rule = rule;
        _totalisticRule = rule as TotalisticRule;
        _nontotalisticRule = rule as NontotalisticRule;

        _nextCellStateCalculator = GetNextCellStateCalculatorByRule(rule);
    }

    public byte CalculateNextCellState(byte[,] world, Point cellPosition) =>
        _nextCellStateCalculator(world, cellPosition);

    #region Private methods
    private NextCellStateCalculator GetNextCellStateCalculatorByRule(Rule rule) => rule switch
    {
        TotalisticRule => CalculateNextCellStateUsingTotalisticRule,
        NontotalisticRule => CalculateNextCellStateUsingNontotalisticRule,
        _ => CalculateNextCellStateUsingUnconditionalRule
    };

    private byte CalculateNextCellStateUsingUnconditionalRule(byte[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;

    private byte CalculateNextCellStateUsingTotalisticRule(byte[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition) && ShouldTotalisticRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;
    
    private byte CalculateNextCellStateUsingNontotalisticRule(byte[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition) && ShouldNontotalisticRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;

    private bool ShouldUnconditionalRuleBeApplied(byte[,] world, Point cellPosition)
    {
        var currentCellState = world[cellPosition.X, cellPosition.Y];
        return currentCellState == _rule.TargetCellState
               && Random.Shared.NextDouble() > _rule.ApplicationProbability;
    }
    
    private bool ShouldTotalisticRuleBeApplied(byte[,] world, Point cellPosition)
    {
        var cellNeighborCountInSpecifiedState = CountCellNeighborsInSpecifiedState(world, cellPosition);
        return _totalisticRule!.NeighborCellCountSet.Contains(cellNeighborCountInSpecifiedState);
    }
    
    private bool ShouldNontotalisticRuleBeApplied(byte[,] world, Point cellPosition) =>
        AreAllCellNeighborsHasSpecifiedState(world, cellPosition);

    private int CountCellNeighborsInSpecifiedState(byte[,] world, Point targetCellPosition)
    {
        var positionShifts = _totalisticRule!.CellNeighborhood.UsedPositions;
        return positionShifts.Count(positionShift =>
        {
            var neighborCellPosition = targetCellPosition + positionShift;
            var neighborCellState = world[neighborCellPosition.X, neighborCellPosition.Y];
            return neighborCellState == _totalisticRule.NeighborCellState;
        });
    }
    
    private bool AreAllCellNeighborsHasSpecifiedState(byte[,] world, Point targetCellPosition)
    {
        var positionShifts = _nontotalisticRule!.NeighborCellPositions;
        return positionShifts.All(positionShift =>
        {
            var neighborCellPosition = targetCellPosition + positionShift;
            var neighborCellState = world[neighborCellPosition.X, neighborCellPosition.Y];
            return neighborCellState == _nontotalisticRule.NeighborCellState;
        });
    }
    #endregion
}