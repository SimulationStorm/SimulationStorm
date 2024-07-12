using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DotNext.Collections.Generic;
using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

// Assumption: min possible cell state starts from one
public class GenericCellularAutomation<TCellState> : SimulationBase, IGenericCellularAutomation<TCellState>
    where TCellState : IBinaryInteger<TCellState>, IMinMaxValue<TCellState>
{
    #region Properties
    public CellStateCollection<TCellState> PossibleCellStateCollection
    {
        get => _possibleCellStateCollection;
        set => SetPossibleCellStateCollection(value);
    }

    public RuleSetCollection<TCellState> RuleSetCollection
    {
        get => _ruleSetCollection;
        set => SetRuleSetCollection(value);
    }

    public int NextExecutingRuleSetIndex { get; }

    public Size WorldSize { get; private set; }

    public WorldWrapping WorldWrapping { get; set; }
    #endregion

    #region Fields
    private readonly IRuleExecutorFactory _ruleExecutorFactory;
    
    private readonly IDictionary<Rule<TCellState>, IRuleExecutor<TCellState>> _ruleExecutorByRules =
        new Dictionary<Rule<TCellState>, IRuleExecutor<TCellState>>();

    private RuleSetCollection<TCellState> _ruleSetCollection;
    
    private readonly Queue<RuleSet<TCellState>> _ruleSetQueue = [];

    private readonly int _innerWorldOffset;

    private Rect _innerWorldRect;

    private TCellState[,] _world;
    
    private readonly TCellState _emptyCellState = TCellState.Zero;

    private CellStateCollection<TCellState> _possibleCellStateCollection;

    private bool _areCopiedWorldSidesReset;
    #endregion
    
    public GenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        int maxCellNeighborhoodRadius,
        Size worldSize,
        WorldWrapping worldWrapping,
        CellStateCollection<TCellState> possibleCellStateCollection,
        RuleSetCollection<TCellState> ruleSetCollection)
    {
        _ruleExecutorFactory = ruleExecutorFactory;
        _innerWorldOffset = maxCellNeighborhoodRadius;
        
        ChangeWorldSize(worldSize);
        WorldWrapping = worldWrapping;
        
        PossibleCellStateCollection = possibleCellStateCollection;
        RuleSetCollection = ruleSetCollection;
    }

    #region World size changing
    public bool IsValidWorldSize(Size size) => size.Area >= 1;
    
    public void ChangeWorldSize(Size newWorldSize)
    {
        if (!IsValidWorldSize(newWorldSize))
            throw new ArgumentOutOfRangeException(nameof(newWorldSize));

        _innerWorldRect = new Rect(_innerWorldOffset, _innerWorldOffset,
            newWorldSize.Width + _innerWorldOffset, newWorldSize.Height + _innerWorldOffset);

        WorldSize = new Size(newWorldSize.Width + _innerWorldOffset * 2, newWorldSize.Height + _innerWorldOffset * 2);

        _world = new TCellState[WorldSize.Width, WorldSize.Height];
        
        ChangeInnerWorldCellsToDefaultState();
    }
    #endregion

    #region Possible cell state collection setting
    private void SetPossibleCellStateCollection(CellStateCollection<TCellState> cellStateCollection)
    {
        cellStateCollection.CellStateSet.ForEach(ValidatePossibleCellState);
        _possibleCellStateCollection = cellStateCollection;
    }

    private void ValidatePossibleCellState(TCellState possibleCellState)
    {
        if (possibleCellState <= _emptyCellState)
            throw new ArgumentOutOfRangeException(nameof(possibleCellState), possibleCellState,
                $"The possible cell state must be greater than the {_emptyCellState}.");
    }
    #endregion
    
    #region World wrapping changing
    private void ApplyWorldWrapping()
    {
        switch (WorldWrapping)
        {
            case WorldWrapping.NoWrap:
            {
                if (!_areCopiedWorldSidesReset)
                {
                    ResetCopiedWorldSides();
                    _areCopiedWorldSidesReset = true;
                }
                break;
            }
            case WorldWrapping.Horizontal:
            {
                CopyWorldHorizontalSides();
                _areCopiedWorldSidesReset = false;
                break;
            }
            case WorldWrapping.Vertical:
            {
                CopyWorldVerticalSides();
                _areCopiedWorldSidesReset = false;
                break;
            }
            case WorldWrapping.Both:
            {
                CopyWorldHorizontalSides();
                CopyWorldVerticalSides();
                _areCopiedWorldSidesReset = false;
                break;
            }
        }
    }

    private void CopyWorldHorizontalSides()
    {
        for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
        {
            for (var i = 0; i < _innerWorldOffset; i++)
            {
                // Copy right to left
                _world[i, y] = _world[_innerWorldRect.Right - _innerWorldOffset + i, y];
                // Copy left to right
                _world[_innerWorldRect.Right + i, y] = _world[i + _innerWorldOffset, y];
            }
        }
    }

    private void CopyWorldVerticalSides()
    {
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
        {
            for (var i = 0; i < _innerWorldOffset; i++)
            {
                // Copy bottom to top
                _world[x, i] = _world[x, _innerWorldRect.Bottom - _innerWorldOffset + i];
                // Copy top to bottom
                _world[x, _innerWorldRect.Bottom + i] = _world[x, i + _innerWorldOffset];
            }
        }
    }

    private void ResetCopiedWorldSides()
    {
        // Reset horizontal sides
        for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
        {
            for (var i = 0; i < _innerWorldOffset; i++)
            {
                // Reset left side
                _world[i, y] = _emptyCellState;
                // Reset right side
                _world[_innerWorldRect.Right + i, y] = _emptyCellState;
            }
        }

        // Reset vertical sides
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
        {
            for (var i = 0; i < _innerWorldOffset; i++)
            {
                //Reset top
                _world[x, i] = _emptyCellState;
                // Reset bottom
                _world[x, _innerWorldRect.Bottom + i] = _emptyCellState;
            }
        }
    }
    #endregion

    #region Setting RuleSet collection
    private void SetRuleSetCollection(RuleSetCollection<TCellState> ruleSetCollection)
    {
        ValidateRuleSetCollection(ruleSetCollection);
        _ruleSetCollection = ruleSetCollection;
        
        CreateRuleExecutorByRules();
    }
    
    private void ValidateRuleSetCollection(RuleSetCollection<TCellState> ruleSetCollection) =>
        ruleSetCollection.RuleSets.ForEach(ValidateRuleSet);

    private void ValidateRuleSet(RuleSet<TCellState> ruleSet) =>
        ruleSet.Rules.ForEach(ValidateRule);

    private void ValidateRule(Rule<TCellState> rule)
    {
        ValidateCellState(rule.TargetCellState);
        ValidateCellState(rule.NewCellState);

        if (rule is ConditionalRuleBase<TCellState> conditionalRule)
            ValidateCellState(conditionalRule.NeighborCellState);
    }

    private void ValidateCellState(TCellState cellState)
    {
        if (!PossibleCellStateCollection.CellStateSet.Contains(cellState))
            throw new ArgumentException(
                $"The {nameof(cellState)} must be in the ${nameof(PossibleCellStateCollection)}.",
                nameof(cellState));
    }
    #endregion

    #region Rule executors creation
    private void CreateRuleExecutorByRules()
    {
        _ruleExecutorByRules.Clear();
        
        _ruleSetCollection.RuleSets.ForEach(ruleSet =>
            ruleSet.Rules.ForEach(rule => _ruleExecutorByRules[rule] = CreateRuleExecutorByRule(rule)));
    }

    private IRuleExecutor<TCellState> CreateRuleExecutorByRule(Rule<TCellState> rule) =>
        _ruleExecutorFactory.CreateRuleExecutor(RuleExecutorType.Straightforward, rule);
    #endregion

    public void Reset() => ChangeInnerWorldCellsToDefaultState();

    #region Advancement
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

            GetInnerWorldCellPositions()
                .AsParallel()
                .ForAll(cellPosition =>
                {
                    var newCellState = ruleExecutor.CalculateNextCellState(_world, cellPosition);
                    nextWorld[cellPosition.X, cellPosition.Y] = newCellState;
                });
        });

        _world = (TCellState[,])nextWorld.Clone();
    }
    #endregion
    
    private IEnumerable<Point> GetInnerWorldCellPositions()
    {
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
            for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
                yield return new Point(x, y);
    }

    #region Rule set queue creation
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
    #endregion

    public GenericCellularAutomationSummary<TCellState> Summarize()
    {
        var cellCountByStates = new Dictionary<TCellState, int>();
        for (var cellState = TCellState.MinValue; cellState < PossibleCellStateCount; cellState++)
            cellCountByStates[cellState] = 0;

        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
        {
            for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
            {
                var cellState = _world[x, y];
                cellCountByStates[cellState]++;
            }
        }

        return new GenericCellularAutomationSummary<TCellState>(cellCountByStates);
    }

    #region Saving / restoring
    public GenericCellularAutomationSave<TCellState> Save()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(GenericCellularAutomationSave<TCellState> save)
    {
        throw new NotImplementedException();
    }
    #endregion

    private void ChangeInnerWorldCellsToDefaultState()
    {
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
            for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
                _world[x, y] = _defaultCellState;
    }

    public TCellState GetCellState(Point cell) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset];

    public void SetCellState(Point cell, TCellState newState) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset] = newState;
}