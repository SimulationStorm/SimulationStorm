using System;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public class SimulationRenderingCompletedEventArgs(SimulationCommand? command, TimeSpan elapsedTime) :
    RenderingCompletedEventArgs(elapsedTime)
{
    public SimulationCommand? Command { get; } = command;
}