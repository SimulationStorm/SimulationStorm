using System;
using System.Globalization;

namespace SimulationStorm.Localization.Presentation;

public class CultureChangedEventArgs(CultureInfo previousCulture, CultureInfo newCulture) : EventArgs
{
    public CultureInfo PreviousCulture { get; } = previousCulture;

    public CultureInfo NewCulture { get; } = newCulture;
}