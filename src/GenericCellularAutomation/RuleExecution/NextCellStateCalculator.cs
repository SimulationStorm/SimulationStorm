using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public delegate byte NextCellStateCalculator(byte[,] world, Point cell);