namespace SimulationStorm.Simulation.Presentation.StatusBar;

public class StatusBarOptions
{
    public bool IsCommandProgressVisible { get; init; }

    public bool IsCommandTimeVisible { get; init; }
    
    public bool IsSimulationRenderingProgressVisible { get; init; }

    public bool IsSimulationRenderingTimeVisible { get; init; }

    public bool IsWorldRenderingTimeVisible { get; init; }
}