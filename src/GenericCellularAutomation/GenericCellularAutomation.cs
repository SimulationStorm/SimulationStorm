using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public class GenericCellularAutomation<TCellState> : SimulationBase, IGenericCellularAutomation<TCellState>
    where TCellState :
        IComparable,
        IComparable<TCellState>,
        IEquatable<TCellState>,
        IBinaryInteger<TCellState>,
        IMinMaxValue<TCellState>
{
    #region Properties
    public TCellState PossibleCellStateCount { get; set; }

    public TCellState MaxPossibleCellStateCount => TCellState.MaxValue;
    
    public TCellState DefaultCellState { get; set; }

    private RuleSetCollection<TCellState> _ruleSetCollection;
    public RuleSetCollection<TCellState> RuleSetCollection
    {
        get => _ruleSetCollection;
        set => SetRuleSetCollection(value);
    }

    public Size WorldSize { get; }
    
    public WorldWrapping WorldWrapping { get; set; }
    #endregion

    #region Fields
    private readonly IRuleExecutorFactory _ruleExecutorFactory;
    
    private readonly IDictionary<Rule<TCellState>, IRuleExecutor<TCellState>> _ruleExecutorByRules =
        new Dictionary<Rule<TCellState>, IRuleExecutor<TCellState>>();

    private readonly Queue<RuleSet<TCellState>> _ruleSetQueue = [];

    private TCellState[,] _world;
    #endregion
    
    public GenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        TCellState possibleCellStateCount,
        RuleSetCollection<TCellState> ruleSetCollection)
    {
        _ruleExecutorFactory = ruleExecutorFactory;
        
        PossibleCellStateCount = possibleCellStateCount; // Todo: validate?
        RuleSetCollection = ruleSetCollection;
    }

    private void SetRuleSetCollection(RuleSetCollection<TCellState> ruleSetCollection)
    {
        ValidateRuleSetCollection(ruleSetCollection);
        _ruleSetCollection = ruleSetCollection;
        
        CreateRuleExecutorByRules();
    }

    private void CreateRuleExecutorByRules()
    {
        _ruleExecutorByRules.Clear();
        
        _ruleSetCollection.RuleSets.ForEach(ruleSet =>
            ruleSet.Rules.ForEach(rule => _ruleExecutorByRules[rule] = CreateRuleExecutorByRule(rule)));
    }

    private IRuleExecutor<TCellState> CreateRuleExecutorByRule(Rule<TCellState> rule) =>
        _ruleExecutorFactory.CreateRuleExecutor(RuleExecutorType.Straightforward, rule);
    
    private void ValidateRuleSetCollection(RuleSetCollection<TCellState> ruleSetCollection) =>
        ruleSetCollection.RuleSets.ForEach(ValidateRuleSet);

    private void ValidateRuleSet(RuleSet<TCellState> ruleSet) =>
        ruleSet.Rules.ForEach(ValidateRule);

    private void ValidateRule(Rule<TCellState> rule)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rule.TargetCellState);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(rule.TargetCellState, PossibleCellStateCount);
        
        ArgumentOutOfRangeException.ThrowIfNegative(rule.NewCellState);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(rule.NewCellState, PossibleCellStateCount);

        if (rule is not ConditionalRuleBase<TCellState> conditionalRule)
            return;
        
        ArgumentOutOfRangeException.ThrowIfNegative(conditionalRule.NeighborCellState);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(conditionalRule.NeighborCellState, PossibleCellStateCount);
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    // Todo: extend advanceable simulation adding CanAdvance() ?
    public bool CanAdvance() => _ruleSetQueue.Count > 0;
    
    public void Advance()
    {
        if (!CanAdvance())
            RecreateRuleSetQueue();
        
        // Todo: Apply field wrapping

        var nextWorld = (TCellState[,])_world.Clone();

        var ruleSet = _ruleSetQueue.Dequeue();
        ruleSet.Rules.ForEach(rule =>
        {
            var ruleExecutor = _ruleExecutorByRules[rule];

            GetAllWorldCellPositions()
                .AsParallel()
                .ForAll(cellPosition =>
                {
                    var newCellState = ruleExecutor.CalculateNextCellState(_world, cellPosition);
                    nextWorld[cellPosition.X, cellPosition.Y] = newCellState;
                });
        });

        _world = (TCellState[,])nextWorld.Clone();
    }
    
    private IEnumerable<Point> GetAllWorldCellPositions()
    {
        for (var x = 0; x < WorldSize.Width; x++)
            for (var y = 0; y < WorldSize.Height; y++)
                yield return new Point(x, y);
    }

    private void RecreateRuleSetQueue()
    {
        _ruleSetQueue.Clear();
        
        var ruleSets = CreateRuleSetStream();
        ruleSets.ForEach(_ruleSetQueue.Enqueue);
    }
    
    private IEnumerable<RuleSet<TCellState>> CreateRuleSetStream() => Enumerable
        .Repeat(RuleSetCollection.RuleSets, RuleSetCollection.RepetitionCount)
            .SelectMany(ruleSets => ruleSets)
        .Select(ruleSet => Enumerable.Repeat(ruleSet, ruleSet.RepetitionCount))
            .SelectMany(ruleSets => ruleSets);

    public GenericCellularAutomationSummary<TCellState> Summarize()
    {
        var cellCountByStates = new Dictionary<TCellState, int>();
        for (var cellState = TCellState.MinValue; cellState < PossibleCellStateCount; cellState++)
            cellCountByStates[cellState] = 0;
        
        for (var x = 1; x < WorldSize.Width - 1; x++)
            for (var y = 1; y < WorldSize.Height - 1; y++)
                cellCountByStates[_world[x, y]]++; // Todo: beautify it

        return new GenericCellularAutomationSummary<TCellState>(cellCountByStates);
    }

    public GenericCellularAutomationSave<TCellState> Save()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(GenericCellularAutomationSave<TCellState> save)
    {
        throw new NotImplementedException();
    }

    public bool IsValidWorldSize(Size size) => size.Area >= 1;

    public void ChangeWorldSize(Size newSize)
    {
        throw new NotImplementedException();
    }

    public TCellState GetCellState(Point cell) => _world[cell.X, cell.Y];

    public void SetCellState(Point cell, TCellState newState) => _world[cell.X, cell.Y] = newState;
}