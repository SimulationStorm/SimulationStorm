using System.Collections.Generic;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.GameOfLife.Presentation.Commands;

public class PlacePatternCommand(GameOfLifePattern pattern, IEnumerable<Point> positions, bool placeWithOverlay) :
    SimulationCommand("PlacePattern", true)
{
    public GameOfLifePattern Pattern { get; } = pattern;

    public IEnumerable<Point> Positions { get; } = positions;

    public bool PlaceWithOverlay { get; } = placeWithOverlay;
}