using System;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.Viewport;

/// <inheritdoc />
public class WorldViewport : IWorldViewport
{
    /// <inheritdoc/>
    public Size Size
    {
        get => _size;
        set
        {
            var previousSize = _size;
            _size = value;
            SizeChanged?.Invoke(this, new ViewportSizeChangedEventArgs(previousSize, _size));
        }
    }

    /// <inheritdoc/>
    public event EventHandler<ViewportSizeChangedEventArgs>? SizeChanged;

    private Size _size;
}