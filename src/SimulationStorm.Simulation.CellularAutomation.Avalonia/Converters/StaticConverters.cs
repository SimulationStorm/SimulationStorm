using Avalonia.Data.Converters;

namespace SimulationStorm.Simulation.CellularAutomation.Avalonia.Converters;

public static class StaticConverters
{
    public static readonly IValueConverter WorldWrappingToString = new WorldWrappingToStringConverter();
}