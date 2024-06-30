using System;
using SimulationStorm.Graphics;

namespace SimulationStorm.Presentation.Colors;

public interface IUiColorProvider
{
    Color BackgroundColor { get; }

    event EventHandler<UiColorChangedEventArgs> BackgroundColorChanged;
}