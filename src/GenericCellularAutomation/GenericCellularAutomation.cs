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
    public int NextExecutingRuleSetIndex { get; private set; }

    public Size WorldSize { get; private set; }

    public WorldWrapping WorldWrapping { get; set; }

    public Range<GcaCellState> CellStateRange => new(EmptyCellState + 1, _maxCellState);
    
    public CellStateCollection CellStateCollection
    {
        get => _cellStateCollection;
        set => SetCellStateCollection(value);
    }

    public RuleSetCollection RuleSetCollection
    {
        get => _ruleSetCollection;
        set => SetRuleSetCollection(value);
    }
    #endregion

    #region Fields
    #region World
    private readonly int _innerWorldOffset;

    private Rect _innerWorldRect;

    private GcaCellState[,] _world = null!;
    
    private bool _areCopiedWorldSidesReset;
    #endregion

    #region Cell states
    private const GcaCellState EmptyCellState = 0;

    private readonly GcaCellState _maxCellState;

    private CellStateCollection _cellStateCollection = null!;
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
        GcaCellState maxCellState,
        WorldWrapping worldWrapping,
        CellStateCollection cellStateCollection,
        RuleSetCollection ruleSetCollection)
    {
        _ruleExecutorFactory = ruleExecutorFactory;
        _innerWorldOffset = maxCellNeighborhoodRadius;
        
        ThrowIfCellStateLessThanEmptyCellState(maxCellState);
        _maxCellState = maxCellState;
        
        ChangeWorldSize(worldSize);
        WorldWrapping = worldWrapping;
        
        CellStateCollection = cellStateCollection;
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

        _world = new GcaCellState[WorldSize.Width, WorldSize.Height];
        
        ChangeInnerWorldCellsToDefaultState();
    }
    #endregion

    #region Setting cell state collection
    private void SetCellStateCollection(CellStateCollection cellStateCollection)
    {
        cellStateCollection.CellStateSet.ForEach(ThrowIfCellStateLessThanEmptyCellState);
        _cellStateCollection = cellStateCollection;
        
        CheckAndChangeInnerWorldCellStatesToDefault();
    }

    private static void ThrowIfCellStateLessThanEmptyCellState(GcaCellState cellState)
    {
        if (cellState <= EmptyCellState)
            throw new ArgumentOutOfRangeException(nameof(cellState), cellState,
                $"The possible cell state must be greater than the {EmptyCellState}.");
    }
    
    private void CheckAndChangeInnerWorldCellStatesToDefault() =>
        ForEachInnerWorldCellInParallel(cellPosition =>
        {
            var cellState = _world[cellPosition.X, cellPosition.Y];
            if (!CellStateCollection.CellStateSet.Contains(cellState))
                _world[cellPosition.X, cellPosition.Y] = CellStateCollection.DefaultCellState;
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
        ThrowIfCellStateIsNotInCollection(rule.TargetCellState);
        ThrowIfCellStateIsNotInCollection(rule.NewCellState);

        if (rule.Type is RuleType.Totalistic or RuleType.Nontotalistic)
            ThrowIfCellStateIsNotInCollection(rule.NeighborCellState!.Value);
    }

    private void ThrowIfCellStateIsNotInCollection(GcaCellState cellState)
    {
        if (!CellStateCollection.CellStateSet.Contains(cellState))
            throw new ArgumentException(
                $"The {nameof(cellState)} must be in the ${nameof(CellStateCollection)}.",
                nameof(cellState));
    }
    #endregion

    #region Rule executors creation
    private void CreateRuleExecutorByRules()
    {
        _ruleExecutorByRules.Clear();
        
        RuleSetCollection.RuleSets.ForEach(ruleSet =>
            ruleSet.Rules.ForEach(rule =>
                _ruleExecutorByRules[rule] = CreateRuleExecutorByRule(rule)));
    }

    private IRuleExecutor CreateRuleExecutorByRule(Rule rule) =>
        _ruleExecutorFactory.CreateRuleExecutor(rule);
    #endregion

    #region Advancement
    public bool CanAdvance() => _ruleSetQueue.Count > 0;
    
    public void Advance()
    {
        if (!CanAdvance())
            RecreateRuleSetQueue();
        
        var ruleSet = _ruleSetQueue.Dequeue();
        IncrementNextExecutingRuleSetIndex();

        ApplyWorldWrapping();

        var nextWorld = (GcaCellState[,])_world.Clone();
        
        ruleSet.Rules.ForEach(rule =>
        {
            var ruleExecutor = _ruleExecutorByRules[rule];

            ForEachInnerWorldCellInParallel(cellPosition =>
            {
                var newCellState = ruleExecutor.CalculateNextCellState(_world, cellPosition);
                nextWorld[cellPosition.X, cellPosition.Y] = newCellState;
            });
        });

        _world = (GcaCellState[,])nextWorld.Clone();
    }

    private void IncrementNextExecutingRuleSetIndex()
    {
        NextExecutingRuleSetIndex++;

        if (NextExecutingRuleSetIndex > GetMaxRuleSetIndex())
            NextExecutingRuleSetIndex = 0;
    }

    private int GetMaxRuleSetIndex() => RuleSetCollection.RuleSets.Count - 1;
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

    #region Saving / restoring
    // Todo: save and restore
    public GcaSave Save()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(GcaSave save)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Other methods
    public void Reset() => ChangeInnerWorldCellsToDefaultState();

    public IReadOnlyDictionary<GcaCellState, IEnumerable<Point>> GetAllCellPositionsByStates()
    {
        var cellPositionsByStates = new Dictionary<GcaCellState, ConcurrentBag<Point>>();
        
        CellStateCollection.CellStateSet.ForEach(cellState => cellPositionsByStates[cellState] = []);
        
        ForEachInnerWorldCellInParallel(cellPosition =>
        {
            var cellState = _world[cellPosition.X, cellPosition.Y];
            var cellPositions = cellPositionsByStates[cellState];
            cellPositions.Add(cellPosition);
        });

        return cellPositionsByStates
            .ToDictionary(kv => kv.Key, kv => kv.Value as IEnumerable<Point>);
    }
    
    public GcaSummary Summarize()
    {
        var cellCountByStates = new ConcurrentDictionary<GcaCellState, int>();
        
        CellStateCollection.CellStateSet.ForEach(cellState => cellCountByStates[cellState] = 0);
        
        ForEachInnerWorldCellInParallel(cell =>
        {
            var cellState = _world[cell.X, cell.Y];
            cellCountByStates[cellState]++;
        });

        return new GcaSummary(cellCountByStates);
    }

    // Todo: add validation and conditional branch
    public GcaCellState GetCellState(Point cell) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset];

    public void SetCellState(Point cell, GcaCellState newState) =>
        _world[cell.X + _innerWorldOffset, cell.Y + _innerWorldOffset] = newState;
    #endregion

    #region Helpers
    private void ChangeInnerWorldCellsToDefaultState() =>
        ForEachInnerWorldCellInParallel(cell =>
            _world[cell.X, cell.Y] = CellStateCollection.DefaultCellState);
    
    private void ForEachInnerWorldCellInParallel(Action<Point> action) => GetInnerWorldCellPositions()
        .AsParallel()
        .ForAll(action);

    private IEnumerable<Point> GetInnerWorldCellPositions()
    {
        for (var x = _innerWorldRect.Left; x < _innerWorldRect.Right; x++)
            for (var y = _innerWorldRect.Top; y < _innerWorldRect.Bottom; y++)
                yield return new Point(x, y);
    }
    #endregion
}