using System;
using System.Collections.Generic;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife.Algorithms;

public class BitwiseGameOfLife : GameOfLifeBase
{
    public const int BatchSize = 8;
    
    #region Properties
    public override Size WorldSize => new(_worldWidth - 2, _worldHeight - 2);

    public sealed override GameOfLifeRule Rule
    {
        get => _rule;
        set => SetRule(value);
    }
    #endregion

    #region Fields
    private int _worldWidth, _worldHeight;

    private byte[] _world = null!, // Cells current live states
                   _neighbors = null!; // Neighbors of each cell (on respective positions)

    private bool _areSidesReset = true;

    private readonly IDictionary<GameOfLifeRule, IReadOnlyList<byte>> _cellStatesByRuleLookupTable =
        new Dictionary<GameOfLifeRule, IReadOnlyList<byte>>();

    private GameOfLifeRule _rule = null!;

    private IReadOnlyList<byte> _cellStatesLookupTable = null!;
    #endregion

    public BitwiseGameOfLife(Size size, WorldWrapping wrapping, GameOfLifeRule rule) : base(wrapping)
    {
        ChangeWorldSize(size);
        Rule = rule;
    }

    #region Public methods
    public override bool IsValidWorldSize(Size size) => size.Area % BatchSize is 0;

    public override GameOfLifeSave Save() =>
        new(WorldSize, WorldWrapping, Rule, GameOfLifeAlgorithm.Bitwise, world: (byte[])_world.Clone());

    public override void RestoreState(GameOfLifeSave save)
    {
        if (save.Algorithm is not GameOfLifeAlgorithm.Bitwise || save.World is null)
            throw new ArgumentException($"Save algorithm must be {nameof(GameOfLifeAlgorithm.Bitwise)} " +
                                        $"and {nameof(save.World)} must not be null.", nameof(save));
        
        _worldWidth = save.WorldSize.Width + 2;
        _worldHeight = save.WorldSize.Height + 2;
        _world = (byte[])save.World.Clone();
        _neighbors = new byte[_worldWidth * _worldHeight];

        WorldWrapping = save.WorldWrapping;
        Rule = save.Rule;
    }

    public override GameOfLifeSummary Summarize()
    {
        var aliveCellCount = 0;
        
        for (var i = _worldWidth + 1; i < (_worldHeight - 1) * _worldWidth; i++)
            aliveCellCount += _world[i];
        
        int totalCellCount = WorldSize.Area,
            deadCellCount = totalCellCount - aliveCellCount;
        
        return new GameOfLifeSummary(deadCellCount, aliveCellCount);
    }

    public sealed override void ChangeWorldSize(Size newSize)
    {
        if (newSize.Area is 0 || newSize.Area % BatchSize is not 0)
            throw new ArgumentOutOfRangeException(nameof(newSize), newSize,
                $"The area of the size must be greater than zero and dividable by {BatchSize}.");
            
        _worldWidth = newSize.Width + 2;
        _worldHeight = newSize.Height + 2;

        _world = new byte[_worldWidth * _worldHeight];
        _neighbors = new byte[_worldWidth * _worldHeight];
    }

    public override void Advance()
    {
        ApplyWorldWrapping();
        ClearCellNeighborCounts();
        CountCellNeighbors();
        ApplyRule();
    }

    public override void Reset()
    {
        _world = new byte[_worldWidth * _worldHeight];
        _neighbors = new byte[_worldWidth * _worldHeight];
    }

    public override GameOfLifeCellState GetCellState(Point cell) =>
        (GameOfLifeCellState)_world[(cell.Y + 1) * _worldWidth + cell.X + 1];

    public override void SetCellState(Point cell, GameOfLifeCellState newState) =>
        _world[(cell.Y + 1) * _worldWidth + cell.X + 1] = (byte)newState;
    #endregion

    #region Private methods
    private void SetRule(GameOfLifeRule rule)
    {
        _rule = rule ?? throw new ArgumentNullException(nameof(rule));
        _cellStatesLookupTable = GetCellStatesLookupTableByRule(rule);
    }
    
    #region World wrapping methods
    private void ApplyWorldWrapping()
    {
        switch (WorldWrapping)
        {
            case WorldWrapping.NoWrap:
                if (!_areSidesReset)
                {
                    ResetSides();
                    _areSidesReset = true;
                }
                break;

            case WorldWrapping.Horizontal:
                CopyHorizontalSides();
                _areSidesReset = false;
                break;

            case WorldWrapping.Vertical:
                CopyVerticalSides();
                _areSidesReset = false;
                break;

            case WorldWrapping.Both:
                CopyHorizontalSides();
                CopyVerticalSides();
                _areSidesReset = false;
                break;
        }
    }

    private void CopyHorizontalSides()
    {
        for (var y = 0; y < _worldHeight; y++)
        {
            // Copy right to left
            _world[y * _worldWidth] = _world[y * _worldWidth + (_worldWidth - 2)];
            // Copy left to right
            _world[y * _worldWidth + (_worldWidth - 1)] = _world[y * _worldWidth + 1];
        }
    }

    private void CopyVerticalSides()
    {
        for (var x = 0; x < _worldWidth; x++)
        {
            // Copy bottom to top
            _world[x] = _world[(_worldHeight - 2) * _worldWidth + x];
            // Copy top to bottom
            _world[(_worldHeight - 1) * _worldWidth + x] = _world[_worldWidth + x];
        }
    }

    private void ResetSides()
    {
        for (var y = 0; y < _worldHeight; y++)
        {
            // Reset left
            _world[y * _worldWidth] = 0;
            // Reset right
            _world[y * _worldWidth + (_worldWidth - 1)] = 0;
        }

        for (var x = 0; x < _worldWidth; x++)
        {
            // Reset top
            _world[x] = 0;
            // Reset bottom
            _world[(_worldHeight - 1) * _worldWidth + x] = 0;
        }
    }
    #endregion

    private unsafe void ClearCellNeighborCounts()
    {
        fixed (byte* neighborsPtr = _neighbors)
        {
            for (var i = 0; i < _worldWidth * _worldHeight; i += BatchSize)
                *(ulong*)(neighborsPtr + i) = 0;
        }
    }
    
    private unsafe void CountCellNeighbors()
    {
        fixed (byte* worldPtr = _world, neighborsPtr = _neighbors)
        {
            for (var i = _worldWidth + 1; i < (_worldHeight - 1) * _worldWidth - 1; i += BatchSize)
            {
                var ptr = (ulong*)(neighborsPtr + i);
                *ptr += *(ulong*)(worldPtr + i - _worldWidth - 1);
                *ptr += *(ulong*)(worldPtr + i - _worldWidth);
                *ptr += *(ulong*)(worldPtr + i - _worldWidth + 1);
                *ptr += *(ulong*)(worldPtr + i - 1);
                *ptr += *(ulong*)(worldPtr + i + 1);
                *ptr += *(ulong*)(worldPtr + i + _worldWidth - 1);
                *ptr += *(ulong*)(worldPtr + i + _worldWidth);
                *ptr += *(ulong*)(worldPtr + i + _worldWidth + 1);
            }
        }
    }

    private void ApplyRule()
    {
        var worldWidthMinusOne = _worldWidth - 1;
        
        for (var i = _worldWidth + 1; i < (_worldHeight - 1) * _worldWidth - 1; i++)
        {
            if (i % _worldWidth == worldWidthMinusOne)
            {
                i++;
                continue;
            }
            
            byte cellState = _world[i],
                 cellNeighborCount = _neighbors[i],
                 newCellState = _cellStatesLookupTable[cellState << 4 | cellNeighborCount];

            _world[i] = newCellState;
        }
    }

    private IReadOnlyList<byte> GetCellStatesLookupTableByRule(GameOfLifeRule rule)
    {
        if (_cellStatesByRuleLookupTable.TryGetValue(rule, out var cellStatesLookupTable))
            return cellStatesLookupTable;

        cellStatesLookupTable = CreateCellStatesLookupTable(rule);
        _cellStatesByRuleLookupTable[rule] = cellStatesLookupTable;

        return cellStatesLookupTable;
    }

    private static IReadOnlyList<byte> CreateCellStatesLookupTable(GameOfLifeRule rule)
    {
        var cellStatesLookupTable = new byte[BatchSize * 4];

        for (var i = GameOfLifeRule.MinNeighborCount; i <= GameOfLifeRule.MaxNeighborCount; i++)
        {
            if (rule.IsBornWhen(i))
                cellStatesLookupTable[i] = 1;

            if (rule.IsSurviveWhen(i))
                cellStatesLookupTable[BatchSize * 2 + i] = 1;
        }

        return cellStatesLookupTable;
    }
    #endregion
}