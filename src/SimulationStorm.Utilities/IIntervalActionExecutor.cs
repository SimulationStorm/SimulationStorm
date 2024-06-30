namespace SimulationStorm.Utilities;

public interface IIntervalActionExecutor
{
    bool IsEnabled { get; set; }
    
    int Interval { get; set; }
    
    bool IsExecutionNeeded { get; }

    bool GetIsExecutionNeededAndMoveNext();
    
    void MoveNext();
}