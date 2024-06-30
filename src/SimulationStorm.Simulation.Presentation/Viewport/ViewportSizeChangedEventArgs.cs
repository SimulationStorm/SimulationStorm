using System;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.Viewport;

public class ViewportSizeChangedEventArgs(Size previousSize, Size newSize) : EventArgs
{
    public Size PreviousSize { get; } = previousSize;

    public Size NewSize { get; } = newSize;
}