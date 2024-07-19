namespace GenericCellularAutomation.Rules;

public class Rule
(
    double applicationProbability,
    byte targetCellState,
    byte newCellState)
{
    public double ApplicationProbability { get; } = applicationProbability;

    public byte TargetCellState { get; } = targetCellState;

    public byte NewCellState { get; } = newCellState;
}