using System;
using System.Collections.Generic;
using System.Linq;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife.Algorithms;

public class SmartGameOfLife : GameOfLifeBase
{
    #region Properties
    public override Size WorldSize => new(_worldWidth, _worldHeight);

    public sealed override GameOfLifeRule Rule
    {
        get => _rule;
        set => SetRule(value);
    }
    #endregion
    
    #region Fields
    private int _worldWidth,
                _worldHeight;

    private ISet<Point> _aliveCells = new HashSet<Point>();

    private readonly IDictionary<Point, int> _neighborCountsByCell = new Dictionary<Point, int>();

    private GameOfLifeRule _rule = null!;
    #endregion
    
    public SmartGameOfLife(Size size, WorldWrapping wrapping, GameOfLifeRule rule) : base(wrapping)
    {
        ChangeWorldSize(size);
        Rule = rule;
    }

    #region Public methods
    public override bool IsValidWorldSize(Size size) => size.Area >= 1;

    public override GameOfLifeSave Save() =>
        new(WorldSize, WorldWrapping, Rule, GameOfLifeAlgorithm.Smart, aliveCells: _aliveCells.ToHashSet());

    public override void RestoreState(GameOfLifeSave save)
    {
        if (save.Algorithm is not GameOfLifeAlgorithm.Smart || save.AliveCells is null)
            throw new ArgumentException($"Saving algorithm must be {nameof(GameOfLifeAlgorithm.Smart)} " +
                                        $"and {nameof(save.AliveCells)} must not be null.", nameof(save));
        
        _worldWidth = save.WorldSize.Width;
        _worldHeight = save.WorldSize.Height;
        _aliveCells = save.AliveCells.ToHashSet();

        WorldWrapping = save.WorldWrapping;
        Rule = save.Rule;
    }

    public override GameOfLifeSummary Summarize()
    {
        int totalCellCount = _worldWidth * _worldHeight,
            aliveCellCount = _aliveCells.Count,
            deadCellCount = totalCellCount - aliveCellCount;

        return new GameOfLifeSummary(deadCellCount, aliveCellCount);
    }

    public sealed override void ChangeWorldSize(Size newSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(newSize.Width, 1, nameof(newSize.Width));
        ArgumentOutOfRangeException.ThrowIfLessThan(newSize.Height, 1, nameof(newSize.Height));
        
        _worldWidth = newSize.Width;
        _worldHeight = newSize.Height;
        
        _aliveCells.Clear();
    }

    public override void Advance()
    {
        CountCellsNeighbors();
        ApplyRule();
    }

    public override void Reset() => _aliveCells.Clear();

    public override GameOfLifeCellState GetCellState(Point cell) =>
        _aliveCells.Contains(cell) ? GameOfLifeCellState.Alive : GameOfLifeCellState.Dead;

    public override void SetCellState(Point cell, GameOfLifeCellState newState)
    {
        if (newState is GameOfLifeCellState.Alive)
            _aliveCells.Add(cell);
        else
            _aliveCells.Remove(cell);
    }
    #endregion

    #region Private methods
    private void SetRule(GameOfLifeRule rule) => _rule = rule ?? throw new ArgumentNullException(nameof(rule));

    private void CountCellsNeighbors()
    {
        _neighborCountsByCell.Clear();

        foreach (var cell in _aliveCells)
            CountCellNeighbors(cell);
    }

    private void ApplyRule()
    {
        var nextWorld = new HashSet<Point>();
        
        foreach (var (cell, neighborCount) in _neighborCountsByCell)
        {
            var isCellAlive = _aliveCells.Contains(cell);
            if (!isCellAlive && _rule.IsBornWhen(neighborCount) || isCellAlive && _rule.IsSurviveWhen(neighborCount))
                nextWorld.Add(cell);
        }

        _aliveCells = nextWorld;
    }

    private void CountCellNeighbors(Point cell)
    {
        int x = cell.X,
            y = cell.Y;

        for (var xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (var yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0)
                    continue;

                int neighborX = x + xOffset,
                    neighborY = y + yOffset;

                if (!ApplyWorldWrappingToCell(ref neighborX, ref neighborY))
                    continue;

                var neighbor = new Point(neighborX, neighborY);
                if (!_neighborCountsByCell.TryAdd(neighbor, 1))
                    _neighborCountsByCell[neighbor]++;
            }
        }
    }

    private bool ApplyWorldWrappingToCell(ref int x, ref int y)
    {
        switch (WorldWrapping)
        {
            case WorldWrapping.NoWrap:
                if (x < 0 || x > _worldWidth - 1 || y < 0 || y > _worldHeight - 1)
                    return false;
                break;

            case WorldWrapping.Horizontal:
                x = (x + _worldWidth) % _worldWidth;
                break;

            case WorldWrapping.Vertical:
                y = (y + _worldHeight) % _worldHeight;
                break;

            case WorldWrapping.Both:
                x = (x + _worldWidth) % _worldWidth;
                y = (y + _worldHeight) % _worldHeight;
                break;
        }

        return true;
    }
    #endregion
}