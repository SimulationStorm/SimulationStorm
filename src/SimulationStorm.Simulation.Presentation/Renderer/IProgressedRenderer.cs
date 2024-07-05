using System;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public interface IProgressedRenderer : IRenderer
{
    bool IsRenderingProgressReportingEnabled { get; set; }

    event EventHandler<CancellableProgressChangedEventArgs> RenderingProgressChanged;
}