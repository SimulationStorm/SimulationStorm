using LiveChartsCore.SkiaSharpView;

namespace SimulationStorm.LiveChartsExtensions.Axes;

public class LogarithmicAxisExtended(double @base) : LogaritmicAxis(@base)
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