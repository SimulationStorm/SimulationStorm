using Avalonia.Data.Converters;

namespace SimulationStorm.Primitives.Avalonia.Converters;

public static class PrimitivesConverters
{
    public static readonly IValueConverter SizeToStringConverter = new SizeToStringConverter();
}