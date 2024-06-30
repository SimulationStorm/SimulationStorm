using LiveChartsCore.SkiaSharpView;

namespace SimulationStorm.LiveChartsExtensions.Axes;

public class AxisExtended : Axis
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