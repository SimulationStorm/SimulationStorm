using System.Collections.Generic;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.History;
using SimulationStorm.Simulation.Resetting;
using SimulationStorm.Simulation.Running;
using SimulationStorm.Simulation.Statistics;

namespace SimulationStorm.GameOfLife;

public interface IGameOfLife :
    IAdvanceableSimulation,
    IResettableSimulation,
    ISummarizableSimulation<GameOfLifeSummary>,
    ISaveableSimulation<GameOfLifeSave>,
    IBoundedCellularAutomation<GameOfLifeCellState>
{
    GameOfLifeRule Rule { get; set; }

    IEnumerable<Point> GetAliveCells();

    void PopulateRandomly(double cellBirthProbability);

    void PlacePattern(GameOfLifePattern pattern, Point position, bool placeWithOverlay);
}