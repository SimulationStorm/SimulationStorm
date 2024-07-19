using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotNext.Collections.Generic;
using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation;
using SimulationStorm.Simulation.CellularAutomation;

namespace GenericCellularAutomation;

public sealed class GenericCellularAutomation : SimulationBase, IGenericCellularAutomation
{
    #region Properties
    public CellStateCollection PossibleCellStateCollection
    {
        get => _possibleCellStateCollection;
        set => SetPossibleCellStateCollection(value);
    }

    public RuleSetCollection RuleSetCollection
    {
        get => _ruleSetCollection;
        set => SetRuleSetCollection(value);
    }

    public int NextExecutingRuleSetIndex { get; }

    public Size WorldSize { get; private set; }

    public WorldWrapping WorldWrapping { get; set; }
    #endregion

    #region Fields
    #region World
    private readonly int _innerWorldOffset;

    private Rect _innerWorldRect;

    private byte[,] _world = null!;
    
    private bool _areCopiedWorldSidesReset;
    #endregion

    #region Possible cell states
    private const byte EmptyCellState = 0;

    private CellStateCollection _possibleCellStateCollection = null!;
    #endregion
    
    #region Rules
    private readonly IRuleExecutorFactory _ruleExecutorFactory;
    
    private readonly IDictionary<Rule, IRuleExecutor> _ruleExecutorByRules =
        new Dictionary<Rule, IRuleExecutor>();

    private readonly Queue<RuleSet> _ruleSetQueue = [];
    
    private RuleSetCollection _ruleSetCollection = null!;
    #endregion
    #endregion
    
    public GenericCellularAutomation
    (
        IRuleExecutorFactory ruleExecutorFactory,
        Size worldSize,
        int maxCellNeighborhoodRadius,
        WorldWrapping worldWrapping,
        CellStateCollection possibleCellStateCollection,
        RuleSetCollection ruleSetCollection)
    {
        _ruleExecutorFactory = ruleExecutorFactory;
        _innerWorldOffset = maxCellNeighborhoodRadius;
        
        ChangeWorldSize(worldSize);
        WorldWrapping = worldWrapping;
        
        PossibleCellStateCollection = possibleCellStateCollection;
        RuleSetCollection = ruleSetCollection;
    }
    
    public IReadOnlyDictionary<byte, IEnumerable<Point>> GetAllCellPositionsByStates()
    {
        var cellPositionsByStates = new Dictionary<byte, ConcurrentBag<Point>>();
        
        PossibleCellStateCollection.CellStateSet.ForEach(cellState => cellPositionsByStates[cellState] = []);
        
        ForEachInnerWorldCellInParallel(cellPosition =>
        {
            var cellState = _world[cellPosition.X, cellPosition.Y];
            var cellPositions = cellPositionsByStates[cellState];
            cellPositions.Add(cellPosition);
        });

        return cellPositionsByStates
            .ToDictionary(kv => kv.Key, kv => kv.Value as IEnumerable<Point>);
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

        _world = new byte[WorldSize.Width, WorldSize.Height];
        
        ChangeInnerWorldCellsToDefaultState();
    }
    #endregion

    #region Possible cell state collection setting
    private void SetPossibleCellStateCollection(CellStateCollection cellStateCollection)
    {
        cellStateCollection.CellStateSet.ForEach(ValidatePossibleCellState);
        _possibleCellStateCollection = cellStateCollection;
        
        CheckAndChangeInnerWorldCellStatesToDefault();
    }

    private void ValidatePossibleCellState(byte possibleCellState)
    {
        if (possibleCellState <= EmptyCellState)
            throw new ArgumentOutOfRangeException(nameof(possibleCellState), possibleCellState,
                $"The possible cell state must be greater than the {EmptyCellState}.");
    }
    
    private void CheckAndChangeInnerWorldCellStatesToDefault() =>
        ForEachInnerWorldCellInParallel(cellPosition =>
        {
            var cellState = _world[cellPosition.X, cellPosition.Y];
            if (!PossibleCellStateCollection.CellStateSet.Contains((byte)cellState))
                _world[cellPosition.X, cellPosition.Y] = PossibleCellStateCollection.DefaultCellState;
        });
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
                _world[i, y] = EmptyCellState;
                // Reset right side
                _world[_innerWorldRect.Right + i, y] = EmptyCellState;
            }
        }

        // Reset vertical sides
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
        {
            for (var i = 0; i < _innerWorldOffset; i++)
            {
                //Reset top
                _world[x, i] = EmptyCellState;
                // Reset bottom
                _world[x, _innerWorldRect.Bottom + i] = EmptyCellState;
            }
        }
    }
    #endregion

    #region Setting RuleSet collection
    private void SetRuleSetCollection(RuleSetCollection ruleSetCollection)
    {
        ValidateRuleSetCollection(ruleSetCollection);
        _ruleSetCollection = ruleSetCollection;
        
        CreateRuleExecutorByRules();
    }
    
    private void ValidateRuleSetCollection(RuleSetCollection ruleSetCollection) =>
        ruleSetCollection.RuleSets.ForEach(ValidateRuleSet);

    private void ValidateRuleSet(RuleSet ruleSet) =>
        ruleSet.Rules.ForEach(ValidateRule);

    private void ValidateRule(Rule rule)
    {
        ValidateCellState(rule.TargetCellState);
        ValidateCellState(rule.NewCellState);

        if (rule is ConditionalRuleBase conditionalRule)
            ValidateCellState(conditionalRule.NeighborCellState);
    }

    private void ValidateCellState(byte cellState)
    {
        if (!PossibleCellStateCollection.CellStateSet.Contains((byte)cellState))
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

    private IRuleExecutor CreateRuleExecutorByRule(Rule rule) =>
        _ruleExecutorFactory.CreateRuleExecutor(RuleExecutorType.Straightforward, rule);
    #endregion

    public void Reset() => ChangeInnerWorldCellsToDefaultState();

    #region Advancement
    public bool CanAdvance() => _ruleSetQueue.Count > 0;
    
    public void Advance()
    {
        if (!CanAdvance())
            RecreateRuleSetQueue();
        
        ApplyWorldWrapping();

        var nextWorld = (byte[,])_world.Clone();

        var ruleSet = _ruleSetQueue.Dequeue();
        ruleSet.Rules.ForEach(rule =>
        {
            var ruleExecutor = _ruleExecutorByRules[rule];

            ForEachInnerWorldCellInParallel(cellPosition =>
            {
                var newCellState = ruleExecutor.CalculateNextCellState(_world, cellPosition);
                nextWorld[cellPosition.X, cellPosition.Y] = newCellState;
            });
        });

        _world = (byte[,])nextWorld.Clone();
    }
    #endregion

    #region Rule set queue creation
    private void RecreateRuleSetQueue()
    {
        _ruleSetQueue.Clear();
        
        var ruleSets = CreateRuleSetStream();
        ruleSets.ForEach(_ruleSetQueue.Enqueue);
    }
    
    private IEnumerable<RuleSet> CreateRuleSetStream() => Enumerable
        .Repeat(RuleSetCollection.RuleSets, RuleSetCollection.RepetitionCount)
            .SelectMany(ruleSets => ruleSets)
        .Select(ruleSet => Enumerable.Repeat(ruleSet, ruleSet.RepetitionCount))
            .SelectMany(ruleSets => ruleSets);
    #endregion

    public GenericCellularAutomationSummary Summarize()
    {
        var cellCountByStates = new ConcurrentDictionary<byte, int>();
        
        PossibleCellStateCollection.CellStateSet.ForEach(cellState => cellCountByStates[cellState] = 0);
        
        ForEachInnerWorldCellInParallel(cell =>
        {
            var cellState = _world[cell.X, cell.Y];
            cellCountByStates[cellState]++;
        });

        return new GenericCellularAutomationSummary(cellCountByStates);
    }

    #region Saving / restoring
    public GenericCellularAutomationSave Save()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(GenericCellularAutomationSave save)
    {
        throw new NotImplementedException();
    }
    #endregion

    private void ChangeInnerWorldCellsToDefaultState() =>
        ForEachInnerWorldCellInParallel(cell =>
            _world[cell.X, cell.Y] = PossibleCellStateCollection.DefaultCellState);
    
    private void ForEachInnerWorldCellInParallel(Action<Point> action) => GetInnerWorldCellPositions()
        .AsParallel()
        .ForAll(action);

    private IEnumerable<Point> GetInnerWorldCellPositions()
    {
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
            for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
                yield return new Point(x, y);
    }

    public byte GetCellState(Point cell) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset];

    public void SetCellState(Point cell, byte newState) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset] = newState;
}