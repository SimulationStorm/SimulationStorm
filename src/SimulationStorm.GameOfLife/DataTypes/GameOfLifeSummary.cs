namespace SimulationStorm.GameOfLife.DataTypes;

public class GameOfLifeSummary(int deadCellCount, int aliveCellCount)
{
    public int DeadCellCount { get; } = deadCellCount;

    public int AliveCellCount { get; } = aliveCellCount;
}