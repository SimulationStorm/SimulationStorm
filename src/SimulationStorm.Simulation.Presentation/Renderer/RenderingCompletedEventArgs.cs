using System;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public class RenderingCompletedEventArgs(TimeSpan elapsedTime) : EventArgs
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
}