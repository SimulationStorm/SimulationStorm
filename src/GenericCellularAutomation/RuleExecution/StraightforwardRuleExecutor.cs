using System;
using System.Linq;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public sealed class StraightforwardRuleExecutor : IRuleExecutor
{
    #region Fields
    private readonly Rule _rule;
    
    private readonly NextCellStateCalculator _nextCellStateCalculator;
    #endregion

    public StraightforwardRuleExecutor(Rule rule)
    {
        _rule = rule;
        _nextCellStateCalculator = GetNextCellStateCalculatorByRule(rule);
    }

    public byte CalculateNextCellState(byte[,] world, Point cellPosition) =>
        _nextCellStateCalculator(world, cellPosition);

    #region Private methods
    private NextCellStateCalculator GetNextCellStateCalculatorByRule(Rule rule) => rule.Type switch
    {
        RuleType.Totalistic => CalculateNextCellStateUsingTotalisticRule,
        RuleType.Nontotalistic => CalculateNextCellStateUsingNontotalisticRule,
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
        return _rule.NeighborCellCountSet!.Contains(cellNeighborCountInSpecifiedState);
    }
    
    private bool ShouldNontotalisticRuleBeApplied(byte[,] world, Point cellPosition) =>
        AreSpecifiedCellNeighborsHasSpecifiedState(world, cellPosition);

    private int CountCellNeighborsInSpecifiedState(byte[,] world, Point targetCellPosition)
    {
        var positionShifts = _rule.CellNeighborhood!.UsedPositions;
        return positionShifts.Count(positionShift =>
        {
            var neighborCellPosition = targetCellPosition + positionShift;
            var neighborCellState = world[neighborCellPosition.X, neighborCellPosition.Y];
            return neighborCellState == _rule.NeighborCellState;
        });
    }
    
    private bool AreSpecifiedCellNeighborsHasSpecifiedState(byte[,] world, Point targetCellPosition)
    {
        var positionShifts = _rule.NeighborCellPositionSet!;
        return positionShifts.All(positionShift =>
        {
            var neighborCellPosition = targetCellPosition + positionShift;
            var neighborCellState = world[neighborCellPosition.X, neighborCellPosition.Y];
            return neighborCellState == _rule.NeighborCellState;
        });
    }
    #endregion
}