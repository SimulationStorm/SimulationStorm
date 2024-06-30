using System;
using LiveChartsCore.SkiaSharpView;

namespace SimulationStorm.LiveChartsExtensions.Axes;

public class DateTimeAxisExtended(TimeSpan unit, Func<DateTime, string> formatter) : DateTimeAxis(unit, formatter)
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