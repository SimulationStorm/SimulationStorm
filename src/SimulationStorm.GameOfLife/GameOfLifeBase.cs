using System;
using System.Collections.Generic;
using DotNext;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation;
using SimulationStorm.Simulation.CellularAutomation;

namespace SimulationStorm.GameOfLife;

public abstract class GameOfLifeBase(WorldWrapping worldWrapping) : SimulationBase, IGameOfLife
{
    #region Properties
    public abstract Size WorldSize { get; }
    
    public abstract GameOfLifeRule Rule { get; set; }

    public WorldWrapping WorldWrapping { get; set; } = worldWrapping;
    #endregion

    #region Abstract methods
    public abstract GameOfLifeSave Save();

    public abstract void RestoreState(GameOfLifeSave save);

    public abstract GameOfLifeSummary Summarize();
    
    public abstract void ChangeWorldSize(Size newSize);

    public abstract void Advance();
    
    public abstract void Reset();

    public abstract GameOfLifeCellState GetCellState(Point cell);

    public abstract void SetCellState(Point cell, GameOfLifeCellState newState);

    public abstract bool IsValidWorldSize(Size size);
    #endregion
    
    #region Public nethods
    public IEnumerable<Point> GetAliveCells()
    {
        int width = WorldSize.Width,
            height = WorldSize.Height;
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var cell = new Point(x, y);
                if (GetCellState(cell) is GameOfLifeCellState.Alive)
                    yield return cell;
            }
        }
    }

    public void PopulateRandomly(double cellBirthProbability)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(cellBirthProbability, 0, nameof(cellBirthProbability));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cellBirthProbability, 1, nameof(cellBirthProbability));
        
        int width = WorldSize.Width,
            height = WorldSize.Height;
        
        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                if (Random.Shared.NextBoolean(cellBirthProbability))
                    SetCellState(new Point(x, y), GameOfLifeCellState.Alive);
    }

    public void PlacePattern(GameOfLifePattern pattern, Point position, bool placeWithOverlay)
    {
        // ArgumentOutOfRangeException.ThrowIfLessThan(position.X, 0, nameof(position.X));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(position.X, WorldSize.Width, nameof(position.X));
        //
        // ArgumentOutOfRangeException.ThrowIfLessThan(position.Y, 0, nameof(position.Y));
        // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(position.Y, WorldSize.Height, nameof(position.Y));
        
        int patternWidth = Math.Min(pattern.Size.Width, WorldSize.Width),
            patternHeight = Math.Min(pattern.Size.Height, WorldSize.Height);

        int patternCenterX = Math.Clamp(position.X - patternWidth / 2, 0, WorldSize.Width - patternWidth),
            patternCenterY = Math.Clamp(position.Y - patternHeight / 2, 0, WorldSize.Height - patternHeight);

        for (var x = 0; x < patternWidth; x++)
        {
            for (var y = 0; y < patternHeight; y++)
            {
                var newCellState = pattern.GetCellState(new Point(x, y));
                if (!placeWithOverlay || newCellState is GameOfLifeCellState.Alive)
                    SetCellState(new Point(patternCenterX + x, patternCenterY + y), newCellState);
            }
        }
    }
    #endregion
}