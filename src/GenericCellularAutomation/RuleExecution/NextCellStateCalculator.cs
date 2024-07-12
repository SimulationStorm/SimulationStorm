using System.Numerics;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public delegate TCellState NextCellStateCalculator<TCellState>(TCellState[,] world, Point cell)
    where TCellState : IBinaryInteger<TCellState>;
