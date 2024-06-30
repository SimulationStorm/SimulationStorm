using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife.Algorithms;

public class VectorGameOfLife : GameOfLifeBase
{
    public static readonly int BatchSize = Vector<byte>.Count;
    
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

    private byte[] _world = null!,
                   _neighbors = null!;

    private IReadOnlyDictionary<int, Vector<byte>> _vectorByNeighborCounts = null!;

    private bool _isSidesReset = true;

    private GameOfLifeRule _rule = null!;

    private IReadOnlyList<Vector<byte>> _bornNeighborCountVectors = null!,
                                        _surviveNeighborCountVectors = null!;
    #endregion

    public VectorGameOfLife(Size size, WorldWrapping wrapping, GameOfLifeRule rule) : base(wrapping)
    {
        ChangeWorldSize(size);
        
        InitializeVectorByNeighborCountsDictionary();
        
        Rule = rule;
    }

    #region Public methods
    public override bool IsValidWorldSize(Size size) => size.Area % BatchSize is 0;

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
        
        ResetNeighborCounts();
        CountNeighbors();
        ApplyRule();
    }

    public override void Reset()
    {
        _world = new byte[_worldWidth * _worldHeight];
        _neighbors = new byte[_worldWidth * _worldHeight];
    }

    public override GameOfLifeSave Save() =>
        new(WorldSize, WorldWrapping, Rule, GameOfLifeAlgorithm.Vector, world: (byte[])_world.Clone());

    public override void RestoreState(GameOfLifeSave save)
    {
        if (save.Algorithm is not GameOfLifeAlgorithm.Vector || save.World is null)
            throw new ArgumentException($"Saving algorithm must be {nameof(GameOfLifeAlgorithm.Vector)} " +
                                        $"and {nameof(save.World)} must not be null.", nameof(save));
        
        _worldWidth = save.WorldSize.Width + 2;
        _worldHeight = save.WorldSize.Height + 2;
        _world = save.World;
        _neighbors = new byte[_worldWidth * _worldHeight];
        
        WorldWrapping = save.WorldWrapping;
        Rule = save.Rule;
    }

    public override GameOfLifeSummary Summarize()
    {
        var aliveCellCount = 0;

        for (var i = _worldWidth + 1; i < (_worldHeight - 1) * _worldWidth - 1; i += BatchSize)
            aliveCellCount += Vector.Sum(new Vector<byte>(_world, i));

        int totalCellCount = WorldSize.Area,
            deadCellCount = totalCellCount - aliveCellCount;

        return new GameOfLifeSummary(deadCellCount, aliveCellCount);
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
        _bornNeighborCountVectors = GetBornNeighborCountVectorsByRule(rule);
        _surviveNeighborCountVectors = GetSurviveNeighborCountVectorsByRule(rule);
    }
    
    #region World wrapping methods
    private void ApplyWorldWrapping()
    {
        switch (WorldWrapping)
        {
            case WorldWrapping.NoWrap:
                if (!_isSidesReset)
                {
                    ResetSides();
                    _isSidesReset = true;
                }
                break;

            case WorldWrapping.Horizontal:
                CopyHorizontalSides();
                _isSidesReset = false;
                break;

            case WorldWrapping.Vertical:
                CopyVerticalSides();
                _isSidesReset = false;
                break;

            case WorldWrapping.Both:
                CopyHorizontalSides();
                CopyVerticalSides();
                _isSidesReset = false;
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

    private unsafe void ResetNeighborCounts()
    {
        fixed (byte* neighborsPtr = _neighbors)
            for (var i = 0; i < _worldWidth * _worldHeight; i += BatchSize)
                Vector<byte>.Zero.Store(neighborsPtr + i);
    }

    private unsafe void CountNeighbors()
    {
        fixed (byte* neighborsPtr = _neighbors)
        {
            for (var i = _worldWidth + 1; i < _worldWidth * (_worldHeight - 1) - 1; i += BatchSize + 2)
            {
                var topLeftNeighbors = new Vector<byte>(_world, i - _worldWidth - 1);
                var topNeighbors = new Vector<byte>(_world, i - _worldWidth);
                var topRightNeighbors = new Vector<byte>(_world, i - _worldWidth + 1);
                var leftNeighbors = new Vector<byte>(_world, i - 1);
                var rightNeighbors = new Vector<byte>(_world, i + 1);
                var bottomLeftNeighbors = new Vector<byte>(_world, i + _worldWidth - 1);
                var bottomNeighbors = new Vector<byte>(_world, i + _worldWidth);
                var bottomRightNeighbors = new Vector<byte>(_world, i + _worldWidth + 1);

                var totalNeighbors =
                    topLeftNeighbors + topNeighbors + topRightNeighbors +
                    leftNeighbors + rightNeighbors +
                    bottomLeftNeighbors + bottomNeighbors + bottomRightNeighbors;
                
                totalNeighbors.Store(neighborsPtr + i);
            }
        }
    }

    private unsafe void ApplyRule()
    {
        fixed (byte* worldPtr = _world)
        {
            for (var i = _worldWidth + 1; i < _worldWidth * (_worldHeight - 1); i += BatchSize + 2)
            {
                var neighbors = new Vector<byte>(_neighbors, i);
                var whenBorn = MaskNeighborsVectorWithNeighborCountVectors(neighbors, _bornNeighborCountVectors);
                var whenSurvive = MaskNeighborsVectorWithNeighborCountVectors(neighbors, _surviveNeighborCountVectors);
                
                var currentCells = new Vector<byte>(_world, i);
                var aliveCells = Vector.Equals(currentCells, Vector<byte>.One);
                var aliveAndNeighbours = Vector.BitwiseAnd(aliveCells, whenSurvive);

                var shouldBeAlive = Vector.BitwiseOr(aliveAndNeighbours, whenBorn);
                shouldBeAlive = Vector.BitwiseAnd(shouldBeAlive, Vector<byte>.One);
                shouldBeAlive.Store(worldPtr + i);
            }
        }
    }

    private void InitializeVectorByNeighborCountsDictionary()
    {
        var vectorByNeighborCounts = new Dictionary<int, Vector<byte>>();

        for (var neighborCount = GameOfLifeRule.MinNeighborCount; neighborCount < GameOfLifeRule.MaxNeighborCount; neighborCount++)
        {
            var neighborCountByteArray = new byte[BatchSize];
            Array.Fill(neighborCountByteArray, (byte)neighborCount);
            vectorByNeighborCounts[neighborCount] = new Vector<byte>(neighborCountByteArray);
        }

        _vectorByNeighborCounts = vectorByNeighborCounts;
    }
    
    private static Vector<byte> MaskNeighborsVectorWithNeighborCountVectors
    (
        Vector<byte> neighbors,
        IEnumerable<Vector<byte>> neighborCountVectors)
    {
        return neighborCountVectors
            .Select(vector => Vector.Equals(neighbors, vector))
            .Aggregate(Vector<byte>.Zero, Vector.BitwiseOr);
    }

    private IReadOnlyList<Vector<byte>> GetBornNeighborCountVectorsByRule(GameOfLifeRule rule)
    {
        var vectors = new List<Vector<byte>>(GameOfLifeRule.MaxNeighborCount);
        
        for (var neighborCount = GameOfLifeRule.MinNeighborCount; neighborCount < GameOfLifeRule.MaxNeighborCount; neighborCount++)
            if (rule.IsBornWhen(neighborCount))
                vectors.Add(_vectorByNeighborCounts[neighborCount]);

        return vectors;
    }
    
    private IReadOnlyList<Vector<byte>> GetSurviveNeighborCountVectorsByRule(GameOfLifeRule rule)
    {
        var vectors = new List<Vector<byte>>(GameOfLifeRule.MaxNeighborCount);
        
        for (var neighborCount = GameOfLifeRule.MinNeighborCount; neighborCount < GameOfLifeRule.MaxNeighborCount; neighborCount++)
            if (rule.IsSurviveWhen(neighborCount))
                vectors.Add(_vectorByNeighborCounts[neighborCount]);

        return vectors;
    }
    #endregion
}