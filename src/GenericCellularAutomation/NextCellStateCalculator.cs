using SimulationStorm.Primitives;

namespace GenericCellularAutomation;

public delegate TCellState NextCellStateCalculator<TCellState>(TCellState[,] world, Point cellPosition)
    where TCellState : notnull;