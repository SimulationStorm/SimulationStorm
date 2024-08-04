using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public delegate GcaCellState NextCellStateCalculator(GcaCellState[,] world, Point cell);