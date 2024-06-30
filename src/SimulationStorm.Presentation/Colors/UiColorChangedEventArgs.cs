using System;
using SimulationStorm.Graphics;

namespace SimulationStorm.Presentation.Colors;

public class UiColorChangedEventArgs(Color previousColor, Color newColor) : EventArgs
{
    public Color PreviousColor { get; } = previousColor;

    public Color NewColor { get; } = newColor;
}