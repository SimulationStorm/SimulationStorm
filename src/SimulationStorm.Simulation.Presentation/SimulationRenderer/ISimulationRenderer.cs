using System;
using System.ComponentModel;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public interface ISimulationRenderer : IRenderer, ISimulationCommandCompletedHandler, INotifyPropertyChanged
{
    bool IsRenderingEnabled { get; set; }
    
    int RenderingInterval { get; set; }
    
    new event EventHandler<SimulationRenderingCompletedEventArgs>? RenderingCompleted;
}