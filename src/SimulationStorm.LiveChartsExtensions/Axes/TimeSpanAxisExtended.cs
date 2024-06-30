using System;
using LiveChartsCore.SkiaSharpView;

namespace SimulationStorm.LiveChartsExtensions.Axes;

public class TimeSpanAxisExtended(TimeSpan unit, Func<TimeSpan, string> formatter) : TimeSpanAxis(unit, formatter)
{
    public new string? Name
    {
        get => base.Name;
        set
        {
            base.Name = value;
            OnPropertyChanged();
        }
    }
}