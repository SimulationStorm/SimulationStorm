using System;
using System.ComponentModel;
using SimulationStorm.Simulation.Presentation.Renderer;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public interface ISimulationRenderer : IRenderer, INotifyPropertyChanged
{
    bool IsRenderingEnabled { get; set; }
    
    int RenderingInterval { get; set; }
    
    new event EventHandler<SimulationRenderingCompletedEventArgs>? RenderingCompleted;
}