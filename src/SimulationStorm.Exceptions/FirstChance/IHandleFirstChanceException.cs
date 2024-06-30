namespace SimulationStorm.Exceptions.FirstChance;

public interface IHandleFirstChanceException
{
    void HandleFirstChanceException(object sender, FirstChanceExceptionEventArgs e);
}