using System;

namespace SimulationStorm.AppStates;

public class AppServiceState
{
    public int Id { get; set; }
    
    public int AppStateId { get; set; }
    
    public virtual AppState AppState { get; set; } = null!;
    
    public Type StateType { get; set; } = null!;

    public object State { get; set; } = null!;
}