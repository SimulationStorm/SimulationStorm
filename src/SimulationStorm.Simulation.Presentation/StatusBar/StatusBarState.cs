using System;

namespace SimulationStorm.Simulation.Presentation.StatusBar;

public class StatusBarState
{
    public bool IsCommandProgressVisible { get; init; }

    public bool IsCommandTimeVisible { get; init; }
    
    public bool IsSimulationRenderingProgressVisible { get; init; }

    public bool IsSimulationRenderingTimeVisible { get; init; }

    public bool IsWorldRenderingTimeVisible { get; init; }
    
    public TimeSpan CommandTime { get; init; }
    
    public TimeSpan SimulationRenderingTime { get; init; }
    
    public TimeSpan WorldRenderingTime { get; init; }
}