using System;
using System.Linq;
using System.Numerics;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

// Todo: rule executor base
public sealed class StraightforwardRuleExecutor<TCellState> : IRuleExecutor<TCellState>
    where TCellState : IBinaryInteger<TCellState>
{
    #region Fields
    private readonly Rule<TCellState> _rule;
    private readonly TotalisticRule<TCellState>? _totalisticRule;
    private readonly NontotalisticRule<TCellState>? _nontotalisticRule;
    
    private readonly NextCellStateCalculator<TCellState> _nextCellStateCalculator;
    #endregion

    public StraightforwardRuleExecutor(Rule<TCellState> rule)
    {
        _rule = rule;
        _totalisticRule = rule as TotalisticRule<TCellState>;
        _nontotalisticRule = rule as NontotalisticRule<TCellState>;

        _nextCellStateCalculator = GetNextCellStateCalculatorByRule(rule);
    }

    public TCellState CalculateNextCellState(TCellState[,] world, Point cellPosition) =>
        _nextCellStateCalculator(world, cellPosition);

    #region Private methods
    private NextCellStateCalculator<TCellState> GetNextCellStateCalculatorByRule(Rule<TCellState> rule) => rule switch
    {
        TotalisticRule<TCellState> => CalculateNextCellStateUsingTotalisticRule,
        NontotalisticRule<TCellState> => CalculateNextCellStateUsingNontotalisticRule,
        _ => CalculateNextCellStateUsingUnconditionalRule
    };

    private TCellState CalculateNextCellStateUsingUnconditionalRule(TCellState[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;

    private TCellState CalculateNextCellStateUsingTotalisticRule(TCellState[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition) && ShouldTotalisticRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;
    
    private TCellState CalculateNextCellStateUsingNontotalisticRule(TCellState[,] world, Point cellPosition) =>
        ShouldUnconditionalRuleBeApplied(world, cellPosition) && ShouldNontotalisticRuleBeApplied(world, cellPosition)
            ? _rule.NewCellState : _rule.TargetCellState;

    private bool ShouldUnconditionalRuleBeApplied(TCellState[,] world, Point cellPosition)
    {
        var currentCellState = world[cellPosition.X, cellPosition.Y];
        return currentCellState == _rule.TargetCellState
               && Random.Shared.NextDouble() > _rule.ApplicationProbability;
    }
    
    private bool ShouldTotalisticRuleBeApplied(TCellState[,] world, Point cellPosition)
    {
        var cellNeighborCountInSpecifiedState = CountCellNeighborsInSpecifiedState(world, cellPosition);
        return _totalisticRule!.NeighborCellCountSet.Contains(cellNeighborCountInSpecifiedState);
    }
    
    private bool ShouldNontotalisticRuleBeApplied(TCellState[,] world, Point cellPosition) =>
        AreAllCellNeighborsHasSpecifiedState(world, cellPosition);

    private int CountCellNeighborsInSpecifiedState(TCellState[,] world, Point targetCellPosition)
    {
        var positionShifts = _totalisticRule!.CellNeighborhood.UsedPositions;
        return positionShifts.Count(positionShift =>
        {
            var neighborCellPosition = targetCellPosition + positionShift;
            var neighborCellState = world[neighborCellPosition.X, neighborCellPosition.Y];
            return neighborCellState == _totalisticRule.NeighborCellState;
        });
    }
    
    private bool AreAllCellNeighborsHasSpecifiedState(TCellState[,] world, Point targetCellPosition)
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